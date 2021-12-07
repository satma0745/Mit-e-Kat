namespace Mitekat.Application.Conventions;

using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mitekat.Application.Extensions;

internal class MvcFeatureConvention : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo controllerType) =>
        controllerType.IsActionClass() && controllerType.HasAttribute<FeatureAttribute>();
}
