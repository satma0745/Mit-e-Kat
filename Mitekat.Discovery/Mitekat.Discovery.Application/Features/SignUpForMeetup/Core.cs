namespace Mitekat.Discovery.Application.Features.SignUpForMeetup;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Seedwork.Features.Requesting;

internal record Request(Guid MeetupId) : RequestBase<Unit>
{
    public override bool AuthenticationRequired => true;

    public Request(Guid meetupId, string accessToken)
        : this(meetupId) =>
        AccessToken = accessToken;
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

        if (meetup.SignedUpUsers.Any(user => user.Id == request.CurrentUser.Id))
        {
            return ConflictFailure();
        }
        meetup.SignUp(request.CurrentUser.Id);
        
        await repository.SaveChanges(cancellationToken);
        return Success();
    }
}
