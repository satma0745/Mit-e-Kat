namespace Mitekat.Auth.Helpers.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Mitekat.Auth.Core.Abstractions.Helpers;

internal class TokenHelper : ITokenHelper
{
    private JwtBuilder TokenBuilder => JwtBuilder
        .Create()
        .WithAlgorithm(new HMACSHA512Algorithm())
        .WithSecret(configuration.Secret);

    private readonly AuthConfiguration configuration;

    public TokenHelper(AuthConfiguration configuration) =>
        this.configuration = configuration;
    
    public TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId)
    {
        var accessToken = TokenBuilder
            .AddClaim("sub", userId)
            .AddClaim("exp", DateTimeOffset.UtcNow.Add(configuration.AccessTokenLifetime).ToUnixTimeSeconds())
            .Encode();

        var refreshToken = TokenBuilder
            .AddClaim("sub", userId)
            .AddClaim("jti", refreshTokenId)
            .AddClaim("exp", DateTimeOffset.UtcNow.Add(configuration.RefreshTokenLifetime).ToUnixTimeSeconds())
            .Encode();

        return new TokenPair(accessToken, refreshToken);
    }

    public AccessTokenPayload ParseAccessToken(string accessToken)
    {
        var claims = ParseToken(accessToken);
        if (claims is null)
        {
            return null;
        }
        
        var userId = Guid.Parse(claims["sub"]);
        return new AccessTokenPayload(userId);
    }

    public RefreshTokenPayload ParseRefreshToken(string refreshToken)
    {
        var claims = ParseToken(refreshToken);
        if (claims is null)
        {
            return null;
        }
        
        var userId = Guid.Parse(claims["sub"]);
        var tokenId = Guid.Parse(claims["jti"]);
        return new RefreshTokenPayload(userId, tokenId);
    }

    private IDictionary<string, string> ParseToken(string token)
    {
        try
        {
            var json = TokenBuilder.MustVerifySignature().Decode(token);
            var claims = JsonSerializer.Deserialize<IDictionary<string, object>>(json)!;
            return claims.ToDictionary(claim => claim.Key, claim => claim.Value.ToString());
        }
        catch (Exception)
        {
            return null;
        }
    }
}
