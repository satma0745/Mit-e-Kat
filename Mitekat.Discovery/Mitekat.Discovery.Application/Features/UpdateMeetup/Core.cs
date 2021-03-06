namespace Mitekat.Discovery.Application.Features.UpdateMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<Unit>
{
    public Guid MeetupId { get; init; }

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
        var meetup = await repository.GetSingle(request.MeetupId, cancellationToken);
        if (meetup is null)
        {
            return NotFoundFailure();
        }

        // TODO: Figure out a better way to update an aggregate root based on a request.
        meetup.Update(
            request.Title,
            request.Description,
            request.Speaker,
            TimeSpan.FromMinutes(request.Duration),
            request.StartTime);
        await repository.SaveChanges(cancellationToken);

        return Success();
    }
}
