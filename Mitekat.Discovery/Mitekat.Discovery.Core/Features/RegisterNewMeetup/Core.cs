namespace Mitekat.Discovery.Core.Features.RegisterNewMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Discovery.Core.Abstraction.Repositories;
using Mitekat.Discovery.Core.Seedwork.Features;
using Mitekat.Discovery.Domain.Aggregates.Meetup;

internal record Request(
    string Title,
    string Description,
    string Speaker,
    int Duration,
    DateTime StartTime) : RequestBase<Unit>;

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
