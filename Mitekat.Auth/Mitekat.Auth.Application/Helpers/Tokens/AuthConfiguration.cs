namespace Mitekat.Auth.Application.Helpers.Tokens;

using System;
using Microsoft.Extensions.Configuration;
using Mitekat.Seedwork.Configuration;
using SeedworkAuthConfiguration = Mitekat.Seedwork.Helpers.Tokens.AuthConfiguration;

internal class AuthConfiguration : SeedworkAuthConfiguration, IAuthConfiguration
{
    public TimeSpan AccessTokenLifetime =>
        Configuration
            .GetParameter("Auth:AccessTokenLifetime", "access token lifetime")
            .Required()
            .Integer()
            .ToTimeAsMinutes();

    public TimeSpan RefreshTokenLifetime =>
        Configuration
            .GetParameter("Auth:RefreshTokenLifetime", "refresh token lifetime")
            .Required()
            .Integer()
            .ToTimeAsDays();

    public AuthConfiguration(IConfiguration configuration)
        : base(configuration)
    {
    }
}
