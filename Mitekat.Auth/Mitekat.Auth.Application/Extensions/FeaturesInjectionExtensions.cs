namespace Mitekat.Auth.Application.Extensions;

using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Seedwork.Features.Behaviors;

public static class FeaturesInjectionExtensions
{
    public static IServiceCollection AddFeatures(this IServiceCollection services) =>
        services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehavior<,>));
}
