namespace Mitekat.Discovery.Application.Features.GetMeetupById;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Discovery.Domain.Aggregates.Meetup;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<ViewModel>
{
    public Guid MeetupId { get; }

    public Request(Guid meetupId) =>
        MeetupId = meetupId;
}

internal class ViewModel
{
    public Guid Id { get; init; }
    
    public string Title { get; init; }
    
    public string Description { get; init; }
    
    public string Speaker { get; init; }
    
    // TODO: Fix TimeSpan swagger example.
    public TimeSpan Duration { get; init; }
    
    public DateTime StartTime { get; init; }
    
    public ICollection<Guid> SignedUpUserIds { get; init; }
}

internal class RequestHandler : RequestHandlerBase<Request, ViewModel>
{
    private readonly IMeetupRepository repository;

    public RequestHandler(IMeetupRepository repository) =>
        this.repository = repository;

    public override async Task<Response<ViewModel>> Handle(Request request, CancellationToken cancellationToken)
    {
        var meetup = await repository.GetSingle(request.MeetupId, cancellationToken);
        if (meetup is null)
        {
            return NotFoundFailure();
        }
            
        var viewModel = ToViewModel(meetup);
        return Success(viewModel);
    }

    private static ViewModel ToViewModel(Meetup meetup) =>
        new()
        {
            Id = meetup.Id,
            Title = meetup.Title,
            Description = meetup.Description,
            Speaker = meetup.Speaker,
            Duration = meetup.Duration,
            StartTime = meetup.StartTime,
            SignedUpUserIds = meetup.SignedUpUsers.Select(user => user.Id).ToList()
        };
}
