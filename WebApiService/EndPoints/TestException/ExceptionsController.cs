using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiService.EndPoints.TestException
{
    /// <summary>
    ///     Тестовый контроллер позволяет получить разные типы ошибок в ответах от сервиса
    /// </summary>
    public class ExceptionsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> InternalServerError()
        {
            throw new Exception("Это специально сгенерированный эксепшен на стороне сервера");
        }

        [HttpGet]
        [Authorize(Roles = "AuthorizeError")]
        public async Task<IActionResult> AuthorizeError()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ForbidError()
        {
            return Forbid();
        }

        [HttpGet]
        public async Task<IActionResult> BadRequestError()
        {
            return BadRequest("Это специально сгенерированный эксепшен на стороне сервера");
        }
    }
}