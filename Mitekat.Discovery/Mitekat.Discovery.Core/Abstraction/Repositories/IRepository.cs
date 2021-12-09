namespace Mitekat.Discovery.Core.Abstraction.Repositories;

using System.Threading;
using System.Threading.Tasks;
using Mitekat.Discovery.Domain.Seedwork;

public interface IRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    public Task SaveChanges(CancellationToken cancellationToken);
}
