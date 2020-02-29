using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApiService.Middlewares.ExceptionWrapperMiddleware
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
                LogException(httpContext, ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDescription = GetErrorDescription(context, exception);
            var serializedErrorInfo = JsonConvert.SerializeObject(errorDescription);
            await context.Response.WriteAsync(serializedErrorInfo);
        }

        protected virtual void LogException(HttpContext context, Exception exception)
        {
            using (Logger.BeginScope($"Request id - {context.TraceIdentifier} error  logging:"))
                Logger.LogError($"Something went wrong: {exception}");
        }

        protected virtual HttpErrorDescription GetErrorDescription(HttpContext context, Exception exception)
        {
            var errorDescription = new HttpErrorDescription(context, exception);
            return errorDescription;
        }
    }
}