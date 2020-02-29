using System;
using Microsoft.Extensions.Options;

namespace WebApiService.Authorization
{
    /// <summary>
    ///     Валидатор настроек <see cref="UserIdAuthorizationOptions"/> кастомного валидатора <see cref="UserIdAuthorizationHandler"/>
    /// </summary>
    public class UserIdAuthorizationPostConfigureOptions : IPostConfigureOptions<UserIdAuthorizationOptions>
    {
        /// <inheritdoc />
        public void PostConfigure(string name, UserIdAuthorizationOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.AuthHeaderName))
                throw new ArgumentException($"Настройка {nameof(UserIdAuthorizationOptions.AuthHeaderName)} авторизации не может быть пустой");

            if (string.IsNullOrWhiteSpace(options.AuthHeaderScheme))
                throw new ArgumentException($"Настройка {nameof(UserIdAuthorizationOptions.AuthHeaderScheme)} авторизации не может быть пустой");
        }
    }
}