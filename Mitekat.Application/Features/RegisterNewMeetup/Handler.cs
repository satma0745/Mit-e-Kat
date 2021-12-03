namespace Mitekat.Application.Features.RegisterNewMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Model.Context;
using Mitekat.Model.Entities;

internal class RegisterNewMeetupRequestHandler : IRequestHandler<RegisterNewMeetupRequest>
{
    private readonly MitekatContext context;

    public RegisterNewMeetupRequestHandler(MitekatContext context) =>
        this.context = context;

    public async Task<Unit> Handle(RegisterNewMeetupRequest request, CancellationToken cancellationToken)
    {
        var meetup = new Meetup
        {
            Id = Guid.NewGuid(),
            Title = request.Properties.Title,
            Description = request.Properties.Description,
            Speaker = request.Properties.Speaker,
            Duration = TimeSpan.FromMinutes(request.Properties.Duration),
            StartTime = request.Properties.StartTime
        };
        context.Meetups.Add(meetup);
        
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
