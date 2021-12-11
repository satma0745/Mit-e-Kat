namespace Mitekat.Discovery.Core.Abstraction.Helpers;

using System;

public record AccessTokenPayload(Guid UserId);

public interface ITokenHelper
{
    AccessTokenPayload ParseAccessToken(string accessToken);
}
