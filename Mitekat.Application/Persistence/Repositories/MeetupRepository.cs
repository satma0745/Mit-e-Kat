namespace Mitekat.Application.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mitekat.Application.Persistence.Context;
using Mitekat.Domain.Aggregates.Meetup;

internal class MeetupRepository : IMeetupRepository
{
    private readonly MitekatContext context;

    public MeetupRepository(MitekatContext context) =>
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
