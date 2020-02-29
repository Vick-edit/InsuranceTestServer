using System.Threading.Tasks;

namespace WebApiService.SchedulingServices
{
    /// <summary>
    ///     Интерфейс работ, которые можно выполнять в сервисе, который отрабатывает по времени <see cref="TimedHostedService{TJob}"/>
    /// </summary>
    public interface IServiceJob
    {
        Task Action();
    }
}