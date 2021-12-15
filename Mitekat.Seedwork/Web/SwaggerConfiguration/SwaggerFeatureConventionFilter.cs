namespace Mitekat.Seedwork.Web.SwaggerConfiguration;

using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Web.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerFeatureConventionFilter : IOperationFilter
{
    public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionClass = context.MethodInfo?.DeclaringType;
        if (actionClass is null || !IsFeatureAction(actionClass))
        {
            return;
        }

        var featureAttribute = actionClass.GetAttribute<FeatureAttribute>();
        SetOperationInfo(operation, featureAttribute);
    }

    protected virtual bool IsFeatureAction(Type candidate) =>
        candidate.IsClass &&
        candidate.IsPublic &&
        !candidate.IsAbstract &&
        !candidate.ContainsGenericParameters &&
        candidate.HasAttribute<FeatureAttribute>();

    protected virtual void SetOperationInfo(OpenApiOperation operation, FeatureAttribute featureAttribute)
    {
        operation.Summary = featureAttribute.Action;
        operation.Tags = new List<OpenApiTag>
        {
            new() { Name = featureAttribute.Group }
        };
    }
}
