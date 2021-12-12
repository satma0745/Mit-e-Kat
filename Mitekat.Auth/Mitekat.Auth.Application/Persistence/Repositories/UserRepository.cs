namespace Mitekat.Auth.Application.Persistence.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mitekat.Auth.Application.Persistence.Context;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Persistence.Repositories;

internal class UserRepository : RepositoryBase<AuthContext>, IUserRepository
{
    public UserRepository(AuthContext context)
        : base(context)
    {
    }

    public Task<User> GetSingle(Guid id, CancellationToken cancellationToken) =>
        Context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);

    public Task<User> GetSingle(string username, CancellationToken cancellationToken) =>
        Context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Username == username, cancellationToken);

    public Task<bool> UsernameTaken(string username, CancellationToken cancellationToken) =>
        Context.Users.AnyAsync(user => user.Username == username, cancellationToken);

    public void Add(User user) =>
        Context.Users.Add(user);
}
