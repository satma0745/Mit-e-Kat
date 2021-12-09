namespace Mitekat.Helpers.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Core.Abstractions.Helpers;
using Mitekat.Helpers.Passwords;
using Mitekat.Helpers.Tokens;

public static class HelpersInjectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services) =>
        services
            .AddScoped<ITokenHelper, TokenHelper>()
            .AddScoped<IPasswordHelper, PasswordHelper>();
}
