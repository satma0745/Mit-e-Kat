namespace Mitekat.Auth.Helpers.Tokens;

using System;
using Microsoft.Extensions.Configuration;
using Mitekat.Auth.Core.Seedwork.Configuration;

internal class AuthConfiguration
{
    public string Secret =>
        configuration
            .GetParameter("Auth:Secret", "auth secret")
            .Required();

    public TimeSpan AccessTokenLifetime =>
        configuration
            .GetParameter("Auth:AccessTokenLifetime", "access token lifetime")
            .Required()
            .Integer()
            .ToTimeAsMinutes();

    public TimeSpan RefreshTokenLifetime =>
        configuration
            .GetParameter("Auth:RefreshTokenLifetime", "refresh token lifetime")
            .Required()
            .Integer()
            .ToTimeAsDays();

    private readonly IConfiguration configuration;

    public AuthConfiguration(IConfiguration configuration) =>
        this.configuration = configuration;
}
