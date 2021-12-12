namespace Mitekat.Seedwork.Persistence.Repositories;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public abstract class RepositoryBase<TContext> : IRepository
    where TContext : DbContext
{
    protected virtual TContext Context { get; }

    protected RepositoryBase(TContext context) =>
        Context = context;

    public virtual Task SaveChanges(CancellationToken cancellationToken) =>
        Context.SaveChangesAsync(cancellationToken);
}
