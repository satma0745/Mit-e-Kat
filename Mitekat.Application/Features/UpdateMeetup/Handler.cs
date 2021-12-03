namespace Mitekat.Application.Features.UpdateMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mitekat.Model.Context;

internal class UpdateMeetupRequestHandler : IRequestHandler<UpdateMeetupRequest>
{
    private readonly MitekatContext context;

    public UpdateMeetupRequestHandler(MitekatContext context) =>
        this.context = context;
    
    public async Task<Unit> Handle(UpdateMeetupRequest request, CancellationToken cancellationToken)
    {
        var meetup = await context.Meetups.SingleAsync(meetup => meetup.Id == request.MeetupId, cancellationToken);
        
        meetup.Title = request.Properties.Title;
        meetup.Description = request.Properties.Description;
        meetup.Speaker = request.Properties.Speaker;
        meetup.Duration = TimeSpan.FromMinutes(request.Properties.Duration);
        meetup.StartTime = request.Properties.StartTime;

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
