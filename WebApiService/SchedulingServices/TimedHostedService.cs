using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace WebApiService.SchedulingServices
{
    /// <summary>
    ///     Сервис, который с заданной переодичностью запускается и выполняет какой-то джоб
    /// </summary>
    public class TimedHostedService<TJob> : IHostedService, IDisposable
        where TJob : class, IServiceJob
    {
        private readonly Container _container;
        private readonly Settings _settings;
        private readonly ILogger _logger;

        private Timer _timer;


        public TimedHostedService(Container container, Settings settings, ILogger logger)
        {
            _container = container;
            _settings = settings;
            _logger = logger;
        }


        private async void ScheduledTask(object state)
        {
            try
            {
                using (AsyncScopedLifestyle.BeginScope(_container))
                {
                    var job = _container.GetInstance<TJob>();
                    await job.Action();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }

        ///<inheritdoc/>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(ScheduledTask, null, TimeSpan.Zero, _settings.Interval);
            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            _timer?.Dispose();
        }


        public class Settings
        {
            public readonly TimeSpan Interval;

            public Settings(TimeSpan interval)
            {
                this.Interval = interval;
            }
        }
    }
}