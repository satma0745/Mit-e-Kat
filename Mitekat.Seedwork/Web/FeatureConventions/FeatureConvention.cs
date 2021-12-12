namespace Mitekat.Seedwork.Web.FeatureConventions;

using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Web.Extensions;

public class FeatureConvention : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo candidate) =>
        candidate.IsClass &&
        candidate.IsPublic &&
        !candidate.IsAbstract &&
        !candidate.ContainsGenericParameters &&
        candidate.HasAttribute<FeatureAttribute>();
}
