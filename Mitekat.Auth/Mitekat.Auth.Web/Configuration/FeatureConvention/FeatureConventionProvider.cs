namespace Mitekat.Auth.Web.Configuration.FeatureConvention;

using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mitekat.Auth.Core.Seedwork.Actions;
using Mitekat.Auth.Web.Extensions;

internal class FeatureConventionProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo controllerType) =>
        controllerType.IsActionClass() && controllerType.HasAttribute<FeatureAttribute>();
}
