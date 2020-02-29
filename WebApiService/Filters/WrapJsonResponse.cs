using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiService.DTOs;

namespace WebApiService.Filters
{
    /// <summary>
    ///     Фильтр, оборачивающий все ответы от сервиса в <see cref="ServiceResponseDto"/>
    /// </summary>
    internal class WrapJsonResponse : IAsyncResultFilter
    {
        /// <inheritdoc />
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var responseData = context.HttpContext.Response;
            var realStatusCode = responseData.StatusCode;
            var contextResult = context.Result as ObjectResult;
            if (contextResult == null)
            {
                await next();
                return;
            }

            var realResultData = contextResult.Value;
            var responseDto = new ServiceResponseDto(realResultData, realStatusCode);

            contextResult.StatusCode = 200;
            contextResult.Value = responseDto;
            contextResult.DeclaredType = typeof(ServiceResponseDto);

            await next();
        }
    }
}