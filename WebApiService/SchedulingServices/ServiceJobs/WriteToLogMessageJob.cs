using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApiService.SchedulingServices.ServiceJobs
{
    /// <summary>
    ///     Простая задача, которая выводит кастомное сообщение в лог с заданной переодичностью
    /// </summary>
    public class WriteToLogMessageJob : IServiceJob
    {
        private const LogLevel MSG_LEVEL = LogLevel.Information;
        private const string MSG_BODY = "Данное сообщение выведено задачей, запускающейся по расписанию";

        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;


        public WriteToLogMessageJob(ILogger logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }


        /// <inheritdoc />
        public async Task Action()
        {
            //Писать в лог будем только в dev версии
            if (!_environment.IsDevelopment())
                return;

            _logger.Log(MSG_LEVEL, MSG_BODY);
        }
    }
}