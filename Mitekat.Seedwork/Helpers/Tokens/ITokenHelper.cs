namespace Mitekat.Seedwork.Helpers.Tokens;

using System;

public record AccessTokenPayload(Guid UserId);

public interface ITokenHelper
{
    AccessTokenPayload ParseAccessToken(string accessToken);
}
