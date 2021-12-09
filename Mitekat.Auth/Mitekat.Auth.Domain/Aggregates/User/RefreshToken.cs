namespace Mitekat.Auth.Domain.Aggregates.User;

using System;

public record RefreshToken(Guid TokenId, Guid UserId)
{
    public RefreshToken(Guid userId)
        : this(TokenId: Guid.NewGuid(), userId)
    {
    }
}
