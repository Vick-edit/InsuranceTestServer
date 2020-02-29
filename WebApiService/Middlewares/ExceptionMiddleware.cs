using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApiService.DTOs;

namespace WebApiService.Middlewares
{
    /// <summary>
    ///     Middleware для оборачивания ошибок работы сервера в стандартизированный Json
    /// </summary>
    internal class ExceptionMiddleware : IMiddleware
    {
        protected readonly ILogger Logger;


        public ExceptionMiddleware(ILogger logger)
        {
            Logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Response.HasStarted)
                    throw;

                LogException(httpContext, ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = 200;

            var responseBody = new ServiceResponseDto(exception);
            var serializedErrorInfo = JsonConvert.SerializeObject(responseBody);
            await context.Response.WriteAsync(serializedErrorInfo);
        }

        protected virtual void LogException(HttpContext context, Exception exception)
        {
            using (Logger.BeginScope($"Request id - {context.TraceIdentifier} error  logging:"))
                Logger.LogError($"Something went wrong: {exception}");
        }
    }
}