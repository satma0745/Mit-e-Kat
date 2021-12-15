namespace Mitekat.Discovery.Application.Features.SignUpForMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

[Feature("Meetups", "Sign up for a meetup")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPost("/api/meetups/{meetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Perform([FromRoute] Guid meetupId, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(CreateRequest(meetupId), cancellationToken)
            .ToActionResult(
                onSuccess: _ => NoContent(),
                error => error switch
                {
                    Error.UnauthorizedError => Unauthorized(),
                    Error.NotFoundError => NotFound(),
                    Error.ConflictError => Conflict(),
                    _ => InternalServerError()
                });

    private Request CreateRequest(Guid meetupId) =>
        new()
        {
            MeetupId = meetupId,
            AccessToken = AccessToken
        };
}
