namespace Mitekat.Auth.Helpers.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Auth.Core.Abstractions.Helpers;
using Mitekat.Auth.Core.Seedwork.Configuration;
using Mitekat.Auth.Helpers.Passwords;
using Mitekat.Auth.Helpers.Tokens;

public static class HelpersInjectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services) =>
        services
            .AddConfiguration<AuthConfiguration>()
            .AddScoped(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                return new AuthConfiguration(configuration);
            })
            .AddScoped<ITokenHelper, TokenHelper>()
            .AddScoped<IPasswordHelper, PasswordHelper>();
}
