namespace Mitekat.Discovery.Application.Features.GetAllMeetups;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Discovery.Domain.Aggregates.Meetup;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<ICollection<ViewModel>>
{
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
    
    public int SignedUp { get; init; }
}

internal class RequestHandler : RequestHandlerBase<Request, ICollection<ViewModel>>
{
    private readonly IMeetupRepository repository;

    public RequestHandler(IMeetupRepository repository) =>
        this.repository = repository;

    public override async Task<Response<ICollection<ViewModel>>> Handle(
        Request _,
        CancellationToken cancellationToken)
    {
        var meetups = await repository.GetAll(cancellationToken);
        var viewModels = meetups.Select(ToViewModel).ToList();
        return Success(viewModels);
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
            SignedUp = meetup.SignedUpUsers.Count
        };
}
