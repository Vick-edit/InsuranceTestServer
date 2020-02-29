using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApiService.Middlewares.ExceptionWrapperMiddleware
{
    /// <summary>
    ///     Расширение <see cref="ExceptionMiddleware"/> которое дополняет сообщение дополнительными сведениями,
    ///     которые помогут лучше понять причину ошибки, но которые не стоит выводить на продакшене
    /// </summary>
    internal class TestExceptionMiddleware : ExceptionMiddleware
    {
        /// <inheritdoc />
        public TestExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
            : base(logger) { }

        /// <inheritdoc />
        protected override void LogException(HttpContext context, Exception exception)
        {
            Logger.LogError($"Something went wrong: {exception}");
        }

        /// <inheritdoc />
        protected override HttpErrorDescription GetErrorDescription(HttpContext context, Exception exception)
        {
            var errorDescription = base.GetErrorDescription(context, exception);
            errorDescription.Source = exception?.Source;
            errorDescription.StackTrace = exception?.StackTrace;
            if (exception?.InnerException != null)
            {
                errorDescription.InnerException = GetErrorDescription(context, exception.InnerException);
            }

            return errorDescription;
        }
    }
}