namespace Mitekat.Auth.Helpers.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Auth.Core.Abstractions.Helpers;
using Mitekat.Auth.Helpers.Passwords;
using Mitekat.Auth.Helpers.Tokens;

public static class HelpersInjectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services) =>
        services
            .AddScoped<ITokenHelper, TokenHelper>()
            .AddScoped<IPasswordHelper, PasswordHelper>();
}
