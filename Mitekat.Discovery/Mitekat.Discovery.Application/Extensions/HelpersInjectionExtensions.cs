namespace Mitekat.Discovery.Application.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Seedwork.Configuration;
using Mitekat.Seedwork.Helpers.Tokens;

internal static class HelpersInjectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services) =>
        services
            .AddConfiguration<IAuthConfiguration, AuthConfiguration>()
            .AddScoped<ITokenHelper, TokenHelper>();
}
