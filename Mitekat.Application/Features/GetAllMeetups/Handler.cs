namespace Mitekat.Application.Features.GetAllMeetups;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mitekat.Model.Context;

internal class GetAllMeetupsRequestHandler : IRequestHandler<GetAllMeetupsRequest, ICollection<MeetupViewModel>>
{
    private readonly MitekatContext context;

    public GetAllMeetupsRequestHandler(MitekatContext context) =>
        this.context = context;

    public async Task<ICollection<MeetupViewModel>> Handle(
        GetAllMeetupsRequest request,
        CancellationToken cancellationToken) =>
        await context.Meetups
            .Select(meetup => new MeetupViewModel
            {
                Id = meetup.Id,
                Title = meetup.Title,
                Description = meetup.Description,
                Speaker = meetup.Speaker,
                Duration = meetup.Duration,
                StartTime = meetup.StartTime
            })
            .ToListAsync(cancellationToken);
}
