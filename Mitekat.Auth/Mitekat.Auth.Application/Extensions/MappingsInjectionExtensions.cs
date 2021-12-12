namespace Mitekat.Auth.Application.Extensions;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

internal static class MappingsInjectionExtensions
{
    public static IServiceCollection AddMappings(this IServiceCollection services) =>
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
}
