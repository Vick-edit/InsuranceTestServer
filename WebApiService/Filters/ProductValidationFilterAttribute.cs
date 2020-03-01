using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace WebApiService.Filters
{
    public class ProductValidationFilterAttribute : Attribute, IFilterFactory
    {
        /// <inheritdoc />
        public bool IsReusable => true;


        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var simpleInjector = serviceProvider.GetService<Container>();
            var productValidationFilter = simpleInjector.GetInstance<ProductValidationFilter>();
            return productValidationFilter;
        }
    }
}