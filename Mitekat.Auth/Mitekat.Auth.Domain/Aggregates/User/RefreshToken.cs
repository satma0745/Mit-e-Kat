namespace Mitekat.Auth.Domain.Aggregates.User;

using System;

public class RefreshToken
{
    public Guid TokenId { get; }
    
    public Guid UserId { get; }

    public RefreshToken(Guid userId)
    {
        TokenId = Guid.NewGuid();
        UserId = UserValidation.EnsureValidUserId(userId);
    }
    
    public override int GetHashCode() =>
        TokenId.GetHashCode();

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is RefreshToken otherRefreshToken &&
               otherRefreshToken.TokenId == TokenId;
    }
}
