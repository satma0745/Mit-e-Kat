namespace Mitekat.Discovery.Application.Extensions;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Seedwork.Web.SwaggerConfiguration;

internal static class SwaggerInjectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services
            .AddSwaggerGen(options =>
            {
                options.ConfigureConventions();
                options.ConfigureAuthentication();
            })
            .AddFluentValidationRulesToSwagger();
}
