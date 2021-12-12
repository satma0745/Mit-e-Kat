namespace Mitekat.Auth.Application.Helpers.Tokens;

using System;
using SeedworkTokenHelper = Mitekat.Seedwork.Helpers.Tokens.TokenHelper;

internal class TokenHelper : SeedworkTokenHelper, ITokenHelper
{
    private readonly IAuthConfiguration configuration;

    public TokenHelper(IAuthConfiguration configuration)
        : base(configuration) =>
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
}
