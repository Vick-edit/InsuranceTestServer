using System;
using System.Linq;
using System.Net;
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
            var responseDto = realResultData is ProblemDetails resultAsProblem 
                ? new ServiceResponseDto(resultAsProblem.Status.GetValueOrDefault(), GetProblemDescription(resultAsProblem)) 
                : new ServiceResponseDto(realResultData, realStatusCode);

            contextResult.StatusCode = (int)HttpStatusCode.OK;;
            contextResult.Value = responseDto;
            contextResult.DeclaredType = typeof(ServiceResponseDto);

            await next();
        }

        private string GetProblemDescription(ProblemDetails problemDetails)
        {
            if (!string.IsNullOrWhiteSpace(problemDetails.Detail))
            {
                return problemDetails.Detail;
            }

            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                var allErrors= validationProblemDetails.Errors
                    .SelectMany(kv => kv.Value)
                    .ToArray();

                var allErrorsText = string.Join("; ", allErrors);
                if (!string.IsNullOrWhiteSpace(allErrorsText))
                    return allErrorsText;

            }

            return problemDetails.Title;
        }
    }
}