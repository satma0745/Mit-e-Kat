namespace Mitekat.Discovery.Web.Configuration.Swagger;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal static class SwaggerConfigurationExtensions
{
    // ReSharper disable once InconsistentNaming
    public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder application, bool showSchemas) =>
        application.UseSwaggerUI(options =>
        {
            if (!showSchemas)
            {
                options.DefaultModelsExpandDepth(-1);
            }
        });
    
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services
            .AddSwaggerGen(options =>
            {
                options.ConfigureConventions();
                options.ConfigureAuthentication();
            })
            .AddFluentValidationRulesToSwagger();

    private static void ConfigureConventions(this SwaggerGenOptions options)
    {
        options.CustomSchemaIds(type => type.FullName);
        options.OperationFilter<SwaggerFeatureConventionFilter>();
    }

    private static void ConfigureAuthentication(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Put Your access token here (drop **Bearer** prefix):",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        
        options.OperationFilter<SwaggerAuthenticationRequirementFilter>();
    }
}
