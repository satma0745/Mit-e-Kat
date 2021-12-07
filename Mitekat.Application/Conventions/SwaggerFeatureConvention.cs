namespace Mitekat.Application.Conventions;

using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Mitekat.Application.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class SwaggerFeatureConvention : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionClassType = context.MethodInfo?.DeclaringType;
        if (actionClassType is null || !actionClassType.IsActionClass())
        {
            return;
        }

        var featureAttribute = actionClassType.GetAttribute<FeatureAttribute>();
        if (featureAttribute is null)
        {
            return;
        }

        operation.Summary = featureAttribute.Action;
        operation.Tags = new List<OpenApiTag>
        {
            new() { Name = featureAttribute.Group }
        };
    }
}
