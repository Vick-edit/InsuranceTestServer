using Microsoft.AspNetCore.Mvc;
using WebApiService.Settings;

namespace WebApiService.Extensions
{
    /// <summary>
    ///     Расширение для опция регистрации MVC
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary> Добавить префикс к жёстко заданым роутам контроллеров, чтобы привести к единообразному формату url </summary>
        public static void UseAttributeRoutePrefix(this MvcOptions opts, string routPrefix)
        {
            if (!routPrefix.EndsWith('/'))
                routPrefix = $"{routPrefix}/";

            var prefixRouteAttribute = new RouteAttribute(routPrefix);
            opts.Conventions.Insert(0, new AttributeRouteConvention(prefixRouteAttribute));
        }
    }
}