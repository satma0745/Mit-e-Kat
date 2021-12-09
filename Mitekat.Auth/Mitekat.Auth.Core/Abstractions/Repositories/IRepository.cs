namespace Mitekat.Auth.Core.Abstractions.Repositories;

using System.Threading;
using System.Threading.Tasks;
using Mitekat.Auth.Domain.Seedwork;

public interface IRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    public Task SaveChanges(CancellationToken cancellationToken);
}
