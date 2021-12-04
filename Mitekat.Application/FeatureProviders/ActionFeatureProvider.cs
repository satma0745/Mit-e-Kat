namespace Mitekat.Application.FeatureProviders;

using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mitekat.Application.Seedwork;

internal class ActionFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        // We are not interested in non-classes and abstract or generic classes
        if (!typeInfo.IsClass || typeInfo.IsAbstract || typeInfo.ContainsGenericParameters)
        {
            return false;
        }
        
        // We are not interested in non-public classes
        if (!typeInfo.IsPublic && !typeInfo.IsNestedPublic)
        {
            return false;
        }

        // Each action should be derived from ActionBase
        // TODO: Replace with custom attribute.
        return typeInfo.IsSubclassOf(typeof(ActionBase));
    }
}
