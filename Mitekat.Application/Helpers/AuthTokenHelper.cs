namespace Mitekat.Application.Helpers;

using System;
using System.Collections.Generic;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;

public record TokenPair(string AccessToken, string RefreshToken);

internal record RefreshTokenPayload(Guid UserId, Guid TokenId);

internal class AuthTokenHelper
{
    private JwtBuilder TokenBuilder => JwtBuilder
        .Create()
        .WithAlgorithm(new HMACSHA512Algorithm())
        .WithSecret(secret);

    private readonly string secret;
    private readonly TimeSpan accessTokenLifetime;
    private readonly TimeSpan refreshTokenLifetime;

    public AuthTokenHelper(IConfiguration configuration)
    {
        secret = configuration["Auth:Secret"];

        var accessTokenLifetimeInMinutes = int.Parse(configuration["Auth:AccessTokenLifetime"]);
        accessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);

        var refreshTokenLifeTimeInDays = int.Parse(configuration["Auth:RefreshTokenLifetime"]);
        refreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifeTimeInDays);
    }
    
    public TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId)
    {
        var accessToken = TokenBuilder
            .AddClaim("sub", userId)
            .AddClaim("exp", DateTimeOffset.UtcNow.Add(accessTokenLifetime).ToUnixTimeSeconds())
            .Encode();

        var refreshToken = TokenBuilder
            .AddClaim("sub", userId)
            .AddClaim("jti", refreshTokenId)
            .AddClaim("exp", DateTimeOffset.UtcNow.Add(refreshTokenLifetime).ToUnixTimeSeconds())
            .Encode();

        return new TokenPair(accessToken, refreshToken);
    }

    public RefreshTokenPayload ParseRefreshToken(string refreshToken)
    {
        try
        {
            var json = TokenBuilder.MustVerifySignature().Decode(refreshToken);
            var payload = JsonSerializer.Deserialize<IDictionary<string, object>>(json)!;

            var userId = Guid.Parse(payload["sub"].ToString()!);
            var tokenId = Guid.Parse(payload["jti"].ToString()!);
            return new RefreshTokenPayload(userId, tokenId);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
