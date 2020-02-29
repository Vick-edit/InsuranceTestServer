using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceCore.DataAccess.SettingsEF;
using ServiceCore.Settings;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using WebApiService.Extensions;
using WebApiService.Middlewares.ExceptionWrapperMiddleware;
using WebApiService.SchedulingServices;
using WebApiService.SchedulingServices.ServiceJobs;
using WebApiService.Settings;

namespace WebApiService
{
    public class Startup
    {
        private readonly Container _container;
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            Configuration = configuration;
            CurrentEnvironment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt => opt.UseAttributeRoutePrefix(AppConstants.ROUT_PREFIX))
                .AddNewtonsoftJson(options =>
                {
                    var settings = options.SerializerSettings;

                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    if (CurrentEnvironment.IsDevelopment())
                        settings.Formatting = Formatting.Indented;
                });

            services.AddAuthentication();

            services.AddSimpleInjector(_container, options =>
            {
                options.AddAspNetCore()             // AddAspNetCore() wraps web requests in a Simple Injector scope.
                    .AddControllerActivation();     // Activation of a specific framework type to be created by Simple Injector instead of the built-in configuration system.

                // Allow application components to depend on the
                // non-generic Microsoft.Extensions.Logging.ILogger
                options.AddLogging();

                options.AddHostedService<TimedHostedService<WriteToLogMessageJob>>();
                options.Container.RegisterInstance(new TimedHostedService<WriteToLogMessageJob>
                    .Settings(interval: TimeSpan.FromSeconds(15)));
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // UseSimpleInjector() enables framework services to be injected into application components, resolved by Simple Injector.
            app.UseSimpleInjector(_container);
            SetupDependencies(_container);
            MigrateDataBase(_container, env, logger);

            // �����, ��� ������� ����������� ������ �� ������� ����������� �����������
            // ��� ������, ��� ��������� ��� ���������� ����� ������� ���� �� ����� ������ � ������ �������� ����� ������
            if (env.IsDevelopment())
                app.UseMiddleware<TestExceptionMiddleware>(_container);
            else
                app.UseMiddleware<ExceptionMiddleware>(_container);

            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "API Default",
                    $"{AppConstants.ROUT_PREFIX}/{{controller}}/{{action}}",
                    new { controller = "Health", action = "Get" }
                );
            });
        }


        // ��������� ������������
        internal static void SetupDependencies(Container container)
        {
            container.Register<IServiceProvider>(() => container, Lifestyle.Singleton);
            DISettings.RegisterDependencies(container);

            container.Verify();
        }


        // �������� ��
        internal static void MigrateDataBase(Container container, IWebHostEnvironment env, ILogger logger)
        {
#if NOT_DEMO
            return;
#endif

            using (AsyncScopedLifestyle.BeginScope(container))
            {
                logger.LogInformation("Migrations START");
                logger.LogInformation("Getting Data Base Client...");
                var dbContextFactory = container.GetInstance<IDbContextFactory>();
                using (var dbContext = dbContextFactory.GetApplicationContext())
                {
                    logger.LogInformation("Applying migration...");
                    dbContext.Migrate();
                    logger.LogInformation("Seed data to Data Base...");
                    dbContext.Seed();
                }
                logger.LogInformation("Migrations FINISHED");
            }
        }
    }
}
