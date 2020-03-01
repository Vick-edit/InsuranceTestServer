using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApiService.DTOs;
using WebApiService.Settings;

namespace WebApiService.Authorization
{
    /// <summary>
    ///     Кастомный хэндлер, который позволяет авторизовываться по хэдеру вида Authorization: ClientId 111
    /// </summary>
    public class UserIdAuthorizationHandler : AuthenticationHandler<UserIdAuthorizationOptions>
    {
        /// <inheritdoc />
        public UserIdAuthorizationHandler(IOptionsMonitor<UserIdAuthorizationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }


        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.AuthHeaderName))
                return AuthenticateResult.NoResult();
            if (!AuthenticationHeaderValue.TryParse(Request.Headers[Options.AuthHeaderName], out var headerValue))
                return AuthenticateResult.NoResult();
            if (!headerValue.Scheme.Equals(Options.AuthHeaderScheme, StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            if (!long.TryParse(headerValue.Parameter, out var userId) || userId < Options.MinValue || userId > Options.MaxValue)
                return AuthenticateResult.Fail("Некорректный Id пользователя");

            var claims = new[] { new Claim(AppConstants.USER_ID_CLAIMS, $"{userId}") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        /// <inheritdoc />
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);
            await HandleAuthError();
        }

        /// <inheritdoc />
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            await base.HandleForbiddenAsync(properties);
            await HandleAuthError();
        }

        private async Task HandleAuthError()
        {
            var originalStatusCode = Context.Response.StatusCode;
            var originalStatusMsg = ((HttpStatusCode)originalStatusCode).ToString();
            Context.Response.ContentType = MediaTypeNames.Application.Json;
            Context.Response.StatusCode = (int)HttpStatusCode.OK;

            var responseBody = new ServiceResponseDto(originalStatusCode, originalStatusMsg);
            var serializedResponseDto = JsonConvert.SerializeObject(responseBody);
            await Context.Response.WriteAsync(serializedResponseDto);
        }
    }
}