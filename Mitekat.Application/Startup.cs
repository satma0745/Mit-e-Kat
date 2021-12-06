namespace Mitekat.Application;

using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Helpers;
using Mitekat.Application.Persistence.Context;
using Mitekat.Application.Persistence.Repositories;
using Mitekat.Domain.Aggregates.Meetup;
using Mitekat.Domain.Aggregates.User;

internal class Startup
{
    // TODO: Move to corresponding Dependency Injection Extension methods.
    public static void ConfigureServices(IServiceCollection services) =>
        services
            .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
                options.OperationFilter<SwaggerFeatureConvention>();
            })
            .AddFluentValidationRulesToSwagger()
            .AddDbContext<MitekatContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("PostgreSQL");

                var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
            })
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IMeetupRepository, MeetupRepository>()
            .AddScoped<AuthTokenHelper>()
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddMediatR(Assembly.GetExecutingAssembly())
            .Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressInferBindingSourcesForParameters = true;
            })
            .AddControllers()
            .ConfigureApplicationPartManager(applicationPart =>
            {
                applicationPart.UseFeatureProvider<MvcFeatureConvention>();
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
