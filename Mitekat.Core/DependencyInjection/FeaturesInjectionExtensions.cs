namespace Mitekat.Core.DependencyInjection;

using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Core.Seedwork.Behaviors;

public static class FeaturesInjectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehavior<,>))
            .AddAutoMapper(Assembly.GetExecutingAssembly());

    public static IMvcBuilder AddValidation(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.AddFluentValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
        });
}
