namespace Mitekat.Web.Configuration.FeatureConvention;

using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mitekat.Core.Seedwork.Action;
using Mitekat.Web.Extensions;

internal class FeatureConventionProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo controllerType) =>
        controllerType.IsActionClass() && controllerType.HasAttribute<FeatureAttribute>();
}
