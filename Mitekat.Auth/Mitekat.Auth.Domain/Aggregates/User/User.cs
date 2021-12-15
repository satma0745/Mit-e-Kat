namespace Mitekat.Auth.Domain.Aggregates.User;

using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public Guid Id { get; }
    
    public string Username { get; private set; }
    
    public string DisplayName { get; }
    
    public string Password { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens;
    private readonly List<RefreshToken> refreshTokens;

    public User(string username, string displayName, string password)
        : this(id: Guid.NewGuid(), username, displayName, password)
    {
    }

    private User(Guid id, string username, string displayName, string password)
    {
        Id = UserValidation.EnsureValidUserId(id);
        Username = UserValidation.EnsureValidUserUsername(username);
        DisplayName = UserValidation.EnsureValidUserDisplayName(displayName);
        Password = UserValidation.EnsureValidUserPassword(password);
        
        refreshTokens = new List<RefreshToken>();
    }

    public void AddRefreshToken(RefreshToken refreshToken) =>
        refreshTokens.Add(refreshToken);

    public void ReplaceRefreshToken(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
    {
        _ = oldRefreshToken ?? throw new ArgumentNullException(nameof(oldRefreshToken));
        _ = newRefreshToken ?? throw new ArgumentNullException(nameof(newRefreshToken));

        refreshTokens.Remove(oldRefreshToken);
        refreshTokens.Add(newRefreshToken);
    }

    public RefreshToken GetRefreshToken(Guid tokenId) =>
        refreshTokens.SingleOrDefault(refreshToken => refreshToken.TokenId == tokenId);

    public void ChangeCredentials(string username, string password)
    {
        Username = UserValidation.EnsureValidUserUsername(username);
        Password = UserValidation.EnsureValidUserPassword(password);
        
        // Revoke all issued refresh tokens to force the user to re-authenticate
        refreshTokens.Clear();
    }
}
