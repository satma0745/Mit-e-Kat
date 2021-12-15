namespace Mitekat.Discovery.Application.Features.RegisterNewMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Discovery.Domain.Aggregates.Meetup;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<Unit>
{
    public string Title { get; init; }
    
    public string Description { get; init; }
    
    public string Speaker { get; init; }
    
    public int Duration { get; init; }
    
    public DateTime StartTime { get; init; }
}

internal class RequestHandler : RequestHandlerBase<Request, Unit>
{
    private readonly IMeetupRepository repository;

    public RequestHandler(IMeetupRepository repository) =>
        this.repository = repository;

    public override async Task<Response<Unit>> Handle(Request request, CancellationToken cancellationToken)
    {
        // TODO: Figure out a better way to instantiate an aggregate root from a request.
        var meetup = new Meetup(
            request.Title,
            request.Description,
            request.Speaker,
            TimeSpan.FromMinutes(request.Duration),
            request.StartTime);
        repository.Add(meetup);
            
        await repository.SaveChanges(cancellationToken);
        return Success();
    }
}
