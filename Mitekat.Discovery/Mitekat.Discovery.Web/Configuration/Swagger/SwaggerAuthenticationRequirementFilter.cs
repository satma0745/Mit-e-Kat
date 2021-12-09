namespace Mitekat.Discovery.Web.Configuration.Swagger;

using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class SwaggerAuthenticationRequirementFilter : IOperationFilter
{
    private readonly OpenApiSecurityRequirement authenticationRequirement;

    public SwaggerAuthenticationRequirementFilter() =>
        authenticationRequirement = new OpenApiSecurityRequirement
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

    public void Apply(OpenApiOperation operation, OperationFilterContext context) =>
        operation.Security.Add(authenticationRequirement);
}
