namespace Mitekat.Auth.Persistence.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mitekat.Auth.Core.Abstractions.Repositories;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Auth.Persistence.Context;

internal class UserRepository : IUserRepository
{
    private readonly AuthContext context;

    public UserRepository(AuthContext context) =>
        this.context = context;

    public Task SaveChanges(CancellationToken cancellationToken) =>
        context.SaveChangesAsync(cancellationToken);

    public Task<User> GetSingle(Guid id, CancellationToken cancellationToken) =>
        context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);

    public Task<User> GetSingle(string username, CancellationToken cancellationToken) =>
        context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Username == username, cancellationToken);

    public Task<bool> UsernameTaken(string username, CancellationToken cancellationToken) =>
        context.Users.AnyAsync(user => user.Username == username, cancellationToken);

    public void Add(User user) =>
        context.Users.Add(user);
}
