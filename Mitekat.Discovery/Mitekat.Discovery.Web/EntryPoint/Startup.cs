namespace Mitekat.Discovery.Web.EntryPoint;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Discovery.Core.DependencyInjection;
using Mitekat.Discovery.Helpers.DependencyInjection;
using Mitekat.Discovery.Persistence.DependencyInjection;
using Mitekat.Discovery.Web.Configuration.FeatureConvention;
using Mitekat.Discovery.Web.Configuration.Swagger;
using Mitekat.Discovery.Web.Extensions;

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
