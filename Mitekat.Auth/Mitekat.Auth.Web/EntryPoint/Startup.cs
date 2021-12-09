namespace Mitekat.Auth.Web.EntryPoint;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Auth.Core.DependencyInjection;
using Mitekat.Auth.Helpers.DependencyInjection;
using Mitekat.Auth.Persistence.DependencyInjection;
using Mitekat.Auth.Web.Configuration.FeatureConvention;
using Mitekat.Auth.Web.Configuration.Swagger;
using Mitekat.Auth.Web.Extensions;

internal class Startup
{
    public static void ConfigureServices(IServiceCollection services) =>
        services
            .AddCore()
            .AddHelpers()
            .AddPersistence()
            .AddSwagger()
            .ConfigureBindingConventions()
            .AddControllers()
            .ConfigureControllerConventions()
            .AddValidation();

    public static void Configure(IApplicationBuilder application, IWebHostEnvironment environment) =>
        application
            .If(environment.IsDevelopment, () =>
            {
                application.UseSwagger();
                application.UseSwaggerUI(showSchemas: false);
            })
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());
}
