namespace Mitekat.Discovery.Web.Configuration.FeatureConvention;

using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mitekat.Discovery.Core.Seedwork.Actions;
using Mitekat.Discovery.Web.Extensions;

internal class FeatureConventionProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo controllerType) =>
        controllerType.IsActionClass() && controllerType.HasAttribute<FeatureAttribute>();
}
