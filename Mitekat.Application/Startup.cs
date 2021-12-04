namespace Mitekat.Application;

using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Application.Extensions;
using Mitekat.Application.FeatureProviders;
using Mitekat.Model.Extensions.DependencyInjection;

internal class Startup
{
    // TODO: Move to corresponding Dependency Injection Extension methods.
    public static void ConfigureServices(IServiceCollection services) =>
        services
            .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            })
            .AddFluentValidationRulesToSwagger()
            .AddMitekatContext()
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddMediatR(Assembly.GetExecutingAssembly())
            .Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressInferBindingSourcesForParameters = true;
            })
            .AddControllers()
            .ConfigureApplicationPartManager(applicationPart =>
            {
                applicationPart.FeatureProviders.Add(new ActionFeatureProvider());
            })
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
                application.UseSwaggerUI(options =>
                {
                    options.DefaultModelsExpandDepth(-1);
                });
            })
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());
}
