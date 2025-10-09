using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ASPNETStarter.Server.Controllers;

public class RoutePrefixConvention(string ns, string prefix) : IControllerModelConvention
{
    private readonly string _namespace = ns;
    private readonly AttributeRouteModel _routePrefix = new(new RouteAttribute(prefix));

    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.Namespace == _namespace)
            foreach (var selector in controller.Selectors)
                if (selector.AttributeRouteModel != null)
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                        _routePrefix, selector.AttributeRouteModel);
                else
                    selector.AttributeRouteModel = _routePrefix;
    }
}