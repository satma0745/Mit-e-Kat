namespace Mitekat.Seedwork.Web.SwaggerConfiguration;

using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerAuthenticationRequirementFilter : IOperationFilter
{
    protected virtual OpenApiSecurityRequirement AuthenticationRequirement =>
        new()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        };

    public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (RequiresAuthentication(operation))
        {
            operation.Security.Add(AuthenticationRequirement);
        }
    }

    protected virtual bool RequiresAuthentication(OpenApiOperation _) =>
        true;
}
