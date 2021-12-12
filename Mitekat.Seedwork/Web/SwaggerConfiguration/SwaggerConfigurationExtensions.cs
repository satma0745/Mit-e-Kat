namespace Mitekat.Seedwork.Web.SwaggerConfiguration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerConfigurationExtensions
{
    // ReSharper disable once InconsistentNaming
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        bool hideSchemasSection = false) =>
        application.UseSwaggerUI(options =>
        {
            if (hideSchemasSection)
            {
                options.DefaultModelsExpandDepth(-1);
            }
        });
    
    public static void ConfigureConventions(this SwaggerGenOptions options)
    {
        options.CustomSchemaIds(type => type.FullName);
        options.OperationFilter<SwaggerFeatureConventionFilter>();
    }

    public static void ConfigureAuthentication(
        this SwaggerGenOptions options,
        string message = "Access token (without the **Bearer** prefix):")
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = message,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        
        options.OperationFilter<SwaggerAuthenticationRequirementFilter>();
    }
}
