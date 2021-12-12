namespace Mitekat.Discovery.Application.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Discovery.Domain.Aggregates.Meetup;
using Mitekat.Seedwork.Persistence.Repositories;

public interface IMeetupRepository : IRepository
{
    Task<ICollection<Meetup>> GetAll(CancellationToken cancellationToken);

    Task<Meetup> GetSingle(Guid id, CancellationToken cancellationToken);

    void Add(Meetup meetup);
}
