namespace Mitekat.Discovery.Core.Abstraction.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Discovery.Domain.Aggregates.Meetup;

public interface IMeetupRepository : IRepository<Meetup>
{
    Task<ICollection<Meetup>> GetAll(CancellationToken cancellationToken);

    Task<Meetup> GetSingle(Guid id, CancellationToken cancellationToken);

    void Add(Meetup meetup);
}
