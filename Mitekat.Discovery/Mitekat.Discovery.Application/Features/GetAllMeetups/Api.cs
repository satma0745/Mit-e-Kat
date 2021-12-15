namespace Mitekat.Discovery.Application.Features.GetAllMeetups;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;

[Feature("Meetups", "Get all meetups")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpGet("/api/meetups")]
    [ProducesResponseType(typeof(ICollection<ViewModel>), StatusCodes.Status200OK)]
    public Task<IActionResult> Perform(CancellationToken cancellationToken) =>
        mediator
            .SendAsync(new Request(), cancellationToken)
            .ToActionResult(
                onSuccess: Ok,
                onFailure: _ => InternalServerError());
}
