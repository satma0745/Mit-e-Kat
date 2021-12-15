namespace Mitekat.Discovery.Application.Features.GetMeetupById;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

[Feature("Meetups", "Get meetup by id")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpGet("/api/meetups/{meetupId:guid}")]
    [ProducesResponseType(typeof(ViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> Perform([FromRoute] Guid meetupId, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(new Request(meetupId), cancellationToken)
            .ToActionResult(
                onSuccess: Ok,
                error => error switch
                {
                    Error.NotFoundError => NotFound(),
                    _ => InternalServerError()
                });
}
