namespace Mitekat.Domain.Seedwork;

using System.Threading;
using System.Threading.Tasks;

public interface IRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    public Task SaveChanges(CancellationToken cancellationToken);
}
