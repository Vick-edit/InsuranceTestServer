using System;
using Newtonsoft.Json;

namespace WebApiService.DTOs
{
    /// <summary>
    ///     DTO объект в который оборачиваются любые ответы от сервиса
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceResponseDto
    {
        [JsonProperty("statusCode", Order = 1)]
        public int StatusCode { get; set; }

        [JsonProperty("message", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("data", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }


        public ServiceResponseDto(Exception exception)
        {
            StatusCode = 500;
            Message = $"Something went wrong: {exception}";
        }

        public ServiceResponseDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ServiceResponseDto(object data, int statusCode = 200, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}