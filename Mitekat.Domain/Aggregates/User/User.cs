namespace Mitekat.Domain.Aggregates.User;

using System;
using System.Collections.Generic;
using System.Linq;
using Mitekat.Domain.Assertions;
using Mitekat.Domain.Seedwork;

public class User : IAggregateRoot
{
    public Guid Id { get; }
    
    public string Username { get; }
    
    public string DisplayName { get; }
    
    public string Password { get; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens;
    private readonly List<RefreshToken> refreshTokens;

    public User(Guid id, string username, string displayName, string password)
    {
        Id = Assert.NotEmpty(id);
        Username = Assert.NotNullOrWhiteSpace(username);
        DisplayName = Assert.NotNullOrWhiteSpace(displayName);
        Password = Assert.NotNullOrWhiteSpace(password);
        
        refreshTokens = new List<RefreshToken>();
    }

    public User(string username, string displayName, string password)
        : this(id: Guid.NewGuid(), username, displayName, password)
    {
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
}
