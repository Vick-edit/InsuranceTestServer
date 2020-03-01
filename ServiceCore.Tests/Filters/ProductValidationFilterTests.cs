using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using ServiceCore.Domain.Models;
using ServiceCore.Services.Validations;
using SimpleInjector;
using WebApiService.Filters;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace ServiceCore.Tests.Filters
{
    [TestFixture]
    public class ProductValidationFilterTests
    {
        [Test]
        public async Task ContextWithWrongModel_OnActionExecutionAsync_CreateBadRequestResult()
        {
            //arrange
            var productValidationFilter = new ProductValidationFilter(new List<INameValidator>(), new List<IDescriptionValidator>());

            var autoMockContainer = AutoMockingContainer.GetNew();
            var context = GenerateActionExecutingContext(autoMockContainer);
            context.ModelState.AddModelError("test", "test_error");
            var next = new ActionExecutionDelegate(() => null);

            //act
            await productValidationFilter.OnActionExecutionAsync(context, next);

            //assert
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(BadRequestObjectResult)));
        }

        private ActionExecutingContext GenerateActionExecutingContext(Container autoMockContainer)
        {
            var httpContext = new DefaultHttpContext();
            var routData = new RouteData();
            var actionDescriptor = new ActionDescriptor();
            var actionContext = new ActionContext(httpContext, routData, actionDescriptor);

            var actionArguments = new Dictionary<string, object>()
            {
                {nameof(Product), new Product()}
            };
            var controller = autoMockContainer.GetInstance<Mock<ControllerBase>>();
            var context = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), actionArguments, controller.Object);
            return context;
        }

    }
}