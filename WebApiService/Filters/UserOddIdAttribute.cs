using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiService.Settings;

namespace WebApiService.Filters
{
    /// <summary>
    ///     Фильтр, непропускающий пользователей с чётным ID
    /// </summary>
    public class UserOddIdAttribute : Attribute, IAsyncAuthorizationFilter
    {

        /// <inheritdoc />
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userClaims = context.HttpContext.User.Claims;
            var userIdClaims = userClaims.FirstOrDefault(c => c.Type == AppConstants.USER_ID_CLAIMS);
            if (userIdClaims == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!int.TryParse(userIdClaims.Value, out var userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (userId == 0 || userId % 2 == 0)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}