using System;
using System.IO;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace ServiceCore.DataAccess.SettingsEF
{
    /// <summary>
    ///     Создание конекшена для построения миграций из VisualStudio
    /// </summary>
    internal class DesignTimeConnectionFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        private const string WEB_API_PROJECT_NAME = "WebApiService";

        /// <inheritdoc />
        public ApplicationContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Build config
            var currentProjectPath = Directory.GetCurrentDirectory();
            Debug.WriteLine($"Current project path: {currentProjectPath}");             
            var webApiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), $"../{WEB_API_PROJECT_NAME}");
            Debug.WriteLine($"WebAPI project path: {webApiProjectPath}");
            if (!Directory.Exists(webApiProjectPath))
                throw new Exception($"Не удалось найти проект {WEB_API_PROJECT_NAME}");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(webApiProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get DbContext
            var dbContextConstructor = typeof(ApplicationContext).GetConstructor(new[] { typeof(IConfiguration) });
            var dbContext = (ApplicationContext)dbContextConstructor?.Invoke(new object[] { config });
            return dbContext;
        }
    }
}