namespace Mitekat.Auth.Application.Helpers.Tokens;

using System;
using ISeedworkTokenHelper = Mitekat.Seedwork.Helpers.Tokens.ITokenHelper;

internal record TokenPair(string AccessToken, string RefreshToken);

internal record RefreshTokenPayload(Guid UserId, Guid TokenId);

internal interface ITokenHelper : ISeedworkTokenHelper
{
    TokenPair IssueTokenPair(Guid userId, Guid refreshTokenId);

    RefreshTokenPayload ParseRefreshToken(string refreshToken);
}
