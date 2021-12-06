namespace Mitekat.Application.Persistence.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mitekat.Application.Persistence.Context;
using Mitekat.Domain.Aggregates.User;

internal class UserRepository : IUserRepository
{
    private readonly MitekatContext context;

    public UserRepository(MitekatContext context) =>
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
