namespace Mitekat.Discovery.Application.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mitekat.Discovery.Application.Persistence.Context;
using Mitekat.Discovery.Domain.Aggregates.Meetup;using Mitekat.Seedwork.Persistence.Repositories;

internal class MeetupRepository : RepositoryBase<DiscoveryContext>, IMeetupRepository
{
    public MeetupRepository(DiscoveryContext context)
        : base(context)
    {
    }

    public async Task<ICollection<Meetup>> GetAll(CancellationToken cancellationToken) =>
        await Context.Meetups.ToListAsync(cancellationToken);

    public Task<Meetup> GetSingle(Guid id, CancellationToken cancellationToken) =>
        Context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id, cancellationToken);

    public void Add(Meetup meetup) =>
        Context.Meetups.Add(meetup);
}
