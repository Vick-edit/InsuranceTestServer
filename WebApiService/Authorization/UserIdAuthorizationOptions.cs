using Microsoft.AspNetCore.Authentication;

namespace WebApiService.Authorization
{
    /// <summary>
    ///     Настройки авторизации на базе Id пользователя 
    /// </summary>
    public class UserIdAuthorizationOptions : AuthenticationSchemeOptions
    {
        public long MinValue { get; set; }
        public long MaxValue { get; set; }

        public string AuthHeaderName { get; set; }
        public string AuthHeaderScheme { get; set; }
    }
}