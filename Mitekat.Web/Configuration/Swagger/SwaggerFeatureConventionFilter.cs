namespace Mitekat.Web.Configuration.Swagger;

using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Mitekat.Core.Seedwork.Action;
using Mitekat.Web.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class SwaggerFeatureConventionFilter : IOperationFilter
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
