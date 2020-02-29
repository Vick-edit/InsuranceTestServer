using Microsoft.AspNetCore.Mvc;

namespace WebApiService.EndPoints.TestHealth
{
    /// <summary>
    ///     Контроллер, который позволяет проверить, что сервис в порядке и в работоспособном состоянии
    /// </summary>
    [Route("{controller}")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public bool Get()
        {
            return true;
        }

        [HttpPost]
        public bool Post()
        {
            return true;
        }

        [HttpPut]
        public bool Put()
        {
            return true;
        }

        [HttpDelete]
        public bool Delete()
        {
            return true;
        }

        [HttpPatch]
        public bool Patch()
        {
            return true;
        }
    }
}