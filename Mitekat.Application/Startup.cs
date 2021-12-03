namespace Mitekat.Application;

using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Application.Extensions;
using Mitekat.Model.Extensions.DependencyInjection;

internal class Startup
{
    public static void ConfigureServices(IServiceCollection services) =>
        services
            .AddSwaggerGen()
            .AddFluentValidationRulesToSwagger()
            .AddMitekatContext()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddControllers()
            .AddFluentValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true;
                options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
            });

    public static void Configure(IApplicationBuilder application, IWebHostEnvironment environment) =>
        application
            .If(environment.IsDevelopment, () =>
            {
                application.UseSwagger();
                application.UseSwaggerUI();
            })
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());
}
