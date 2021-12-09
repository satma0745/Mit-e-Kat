namespace Mitekat.Web.EntryPoint;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Core.DependencyInjection;
using Mitekat.Helpers.DependencyInjection;
using Mitekat.Persistence.DependencyInjection;
using Mitekat.Web.Configuration.FeatureConvention;
using Mitekat.Web.Configuration.Swagger;
using Mitekat.Web.Extensions;

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
