namespace Mitekat.Discovery.Helpers.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Discovery.Core.Abstraction.Helpers;
using Mitekat.Discovery.Helpers.Tokens;

public static class HelpersInjectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services) =>
        services.AddScoped<ITokenHelper, TokenHelper>();
}
