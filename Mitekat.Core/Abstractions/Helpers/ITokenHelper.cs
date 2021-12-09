namespace Mitekat.Core.Abstractions.Helpers;

using System;

public record TokenPair(string AccessToken, string RefreshToken);

public record AccessTokenPayload(Guid UserId);

public record RefreshTokenPayload(Guid UserId, Guid TokenId);

public interface ITokenHelper
{
    TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId);

    AccessTokenPayload ParseAccessToken(string accessToken);

    RefreshTokenPayload ParseRefreshToken(string refreshToken);
}
