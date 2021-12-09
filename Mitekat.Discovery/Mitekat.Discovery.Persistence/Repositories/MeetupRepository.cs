namespace Mitekat.Discovery.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mitekat.Discovery.Core.Abstraction.Repositories;
using Mitekat.Discovery.Domain.Aggregates.Meetup;
using Mitekat.Discovery.Persistence.Context;

internal class MeetupRepository : IMeetupRepository
{
    private readonly DiscoveryContext context;

    public MeetupRepository(DiscoveryContext context) =>
        this.context = context;

    public Task SaveChanges(CancellationToken cancellationToken) =>
        context.SaveChangesAsync(cancellationToken);

    public async Task<ICollection<Meetup>> GetAll(CancellationToken cancellationToken) =>
        await context.Meetups.ToListAsync(cancellationToken);

    public Task<Meetup> GetSingle(Guid id, CancellationToken cancellationToken) =>
        context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id, cancellationToken);

    public void Add(Meetup meetup) =>
        context.Meetups.Add(meetup);
}
