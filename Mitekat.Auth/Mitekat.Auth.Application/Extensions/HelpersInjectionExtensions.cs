namespace Mitekat.Auth.Application.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Auth.Application.Helpers.Passwords;
using Mitekat.Auth.Application.Helpers.Tokens;
using Mitekat.Seedwork.Configuration;
using ISeedworkTokenHelper = Seedwork.Helpers.Tokens.ITokenHelper;

public static class HelpersInjectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services) =>
        services
            .AddConfiguration<IAuthConfiguration, AuthConfiguration>()
            .AddScoped<ISeedworkTokenHelper, TokenHelper>()
            .AddScoped<ITokenHelper, TokenHelper>()
            .AddScoped<IPasswordHelper, PasswordHelper>();
}
