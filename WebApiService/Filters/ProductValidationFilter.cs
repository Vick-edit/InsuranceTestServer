using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceCore.Domain.Models;
using ServiceCore.Services.Validations;

namespace WebApiService.Filters
{
    /// <summary>
    ///     Фильтр для применения кастомной валидации на продукте
    /// </summary>
    public class ProductValidationFilter : IAsyncActionFilter
    {
        private readonly IEnumerable<INameValidator> NameValidators;
        private readonly IEnumerable<IDescriptionValidator> DescriptionValidators;

        public ProductValidationFilter(IEnumerable<INameValidator> nameValidators, IEnumerable<IDescriptionValidator> descriptionValidators)
        {
            NameValidators = nameValidators;
            DescriptionValidators = descriptionValidators;
        }

        /// <inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var productParam = context
                .ActionArguments
                .Values
                .OfType<Product>()
                .FirstOrDefault();

            if (productParam == null)
            {
                await next();
                return;
            }

            var nameValidations = NameValidators.Select(v => v.ValidateAsync(productParam.Name));
            var isNameValidChecks = await Task.WhenAll(nameValidations);
            if (isNameValidChecks.Any(v => !v))
                context.ModelState.AddModelError(nameof(Product.Name), "Имя продукта не прошло дополнительные проверки");

            var descriptionValidations = DescriptionValidators.Select(v => v.ValidateAsync(productParam.Description));
            var isDescriptionValidChecks = await Task.WhenAll(descriptionValidations);
            if (isDescriptionValidChecks.Any(v => !v))
                context.ModelState.AddModelError(nameof(Product.Description), "Описание продукта не прошло дополнительные проверки");

            if (!context.ModelState.IsValid)
            {
                var validationProblem = new ValidationProblemDetails(context.ModelState)
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = HttpStatusCode.BadRequest.ToString(),
                };
                context.Result = new BadRequestObjectResult(validationProblem);
                return;
            }
                
            await next();
        }
    }
}