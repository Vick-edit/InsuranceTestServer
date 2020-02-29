using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApiService.Middlewares.ExceptionWrapperMiddleware
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class HttpErrorDescription
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }


        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("stack_trace", NullValueHandling = NullValueHandling.Ignore)]
        public string StackTrace { get; set; }

        [JsonProperty("inner_exception", NullValueHandling = NullValueHandling.Ignore)]
        public HttpErrorDescription InnerException { get; set; }


        public HttpErrorDescription(HttpContext context, Exception exception)
        {
            Title = "One or more internal server errors occurred.";
            StatusCode = context?.Response.StatusCode != null ? (HttpStatusCode)context.Response.StatusCode : HttpStatusCode.InternalServerError;
            Message = exception?.Message;
            TraceId = context?.TraceIdentifier;
        }
    }
}