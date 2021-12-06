namespace Mitekat.Domain.Aggregates.User;

using System;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Domain.Seedwork;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetSingle(Guid id, CancellationToken cancellationToken);

    Task<User> GetSingle(string username, CancellationToken cancellationToken);

    Task<bool> UsernameTaken(string username, CancellationToken cancellationToken);

    void Add(User user);
}
