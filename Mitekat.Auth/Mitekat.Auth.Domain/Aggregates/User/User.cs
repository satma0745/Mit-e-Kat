namespace Mitekat.Auth.Domain.Aggregates.User;

using System;
using System.Collections.Generic;
using System.Linq;
using Mitekat.Auth.Domain.Assertions;
using Mitekat.Auth.Domain.Seedwork;

public class User : IAggregateRoot
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
        Id = Assert.NotEmpty(id);
        Username = Assert.NotNullOrWhiteSpace(username);
        DisplayName = Assert.NotNullOrWhiteSpace(displayName);
        Password = Assert.NotNullOrWhiteSpace(password);
        
        refreshTokens = new List<RefreshToken>();
    }

    public void AddRefreshToken(RefreshToken refreshToken) =>
        refreshTokens.Add(refreshToken);

    public void ReplaceRefreshToken(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
    {
        Assert.NotNull(oldRefreshToken);
        Assert.NotNull(newRefreshToken);

        refreshTokens.Remove(oldRefreshToken);
        refreshTokens.Add(newRefreshToken);
    }

    public RefreshToken GetRefreshToken(Guid tokenId) =>
        refreshTokens.SingleOrDefault(refreshToken => refreshToken.TokenId == tokenId);

    public void ChangeCredentials(string username, string password)
    {
        Username = Assert.NotNullOrWhiteSpace(username);
        Password = Assert.NotNullOrWhiteSpace(password);
        
        // Revoke all issued refresh tokens to force the user to re-authenticate
        refreshTokens.Clear();
    }
}
