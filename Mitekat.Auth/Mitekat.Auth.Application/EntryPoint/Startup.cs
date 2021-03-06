namespace Mitekat.Auth.Application.EntryPoint;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Auth.Application.Extensions;
using Mitekat.Seedwork.Features.DependencyInjection;
using Mitekat.Seedwork.Web.FeatureConventions;
using Mitekat.Seedwork.Web.FluentApi;
using Mitekat.Seedwork.Web.SwaggerConfiguration;

internal class Startup
{
    public static void ConfigureServices(IServiceCollection services) =>
        services
            .AddFeatures()
            .AddHelpers()
            .AddPersistence()
            .AddSwagger()
            .DisableBindingSourcesInference()
            .AddControllers()
            .ConfigureApplicationParts()
            .AddFluentValidation();

    public static void Configure(IApplicationBuilder application, IWebHostEnvironment environment) =>
        application
            .If(environment.IsDevelopment, () =>
            {
                application.UseSwagger();
                application.UseSwaggerUI(hideSchemasSection: true);
            })
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());
}
