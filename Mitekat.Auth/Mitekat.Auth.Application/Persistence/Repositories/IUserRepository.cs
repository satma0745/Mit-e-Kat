namespace Mitekat.Auth.Application.Persistence.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Persistence.Repositories;

internal interface IUserRepository : IRepository
{
    Task<User> GetSingle(Guid id, CancellationToken cancellationToken);

    Task<User> GetSingle(string username, CancellationToken cancellationToken);

    Task<bool> UsernameTaken(string username, CancellationToken cancellationToken);

    void Add(User user);
}
