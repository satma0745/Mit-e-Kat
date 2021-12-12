namespace Mitekat.Seedwork.Persistence.Repositories;

using System.Threading;
using System.Threading.Tasks;

public interface IRepository
{
    public Task SaveChanges(CancellationToken cancellationToken);
}
