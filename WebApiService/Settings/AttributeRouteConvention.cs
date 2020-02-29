using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApiService.Settings
{
    public class AttributeRouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routingPrefix;


        public AttributeRouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _routingPrefix = new AttributeRouteModel(routeTemplateProvider);
        }


        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routingPrefix,
                            selectorModel.AttributeRouteModel);
                    }
                }
            }
        }
    }
}