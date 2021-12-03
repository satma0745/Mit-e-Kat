namespace Mitekat.Application.Features.GetMeetupById;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mitekat.Model.Context;

internal class GetMeetupByIdRequestHandler : IRequestHandler<GetMeetupByIdRequest, MeetupViewModel>
{
    private readonly MitekatContext context;

    public GetMeetupByIdRequestHandler(MitekatContext context) =>
        this.context = context;
    
    public async Task<MeetupViewModel> Handle(GetMeetupByIdRequest request, CancellationToken cancellationToken)
    {
        var meetup = await context.Meetups.SingleAsync(meetup => meetup.Id == request.MeetupId, cancellationToken);

        return new MeetupViewModel
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Speaker = meetup.Speaker,
            Duration = meetup.Duration,
            StartTime = meetup.StartTime
        };
    }
}
