namespace Mitekat.Application.Features.Meetups.GetAllMeetups;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Seedwork;

[Feature("Meetups", "Get all meetups")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpGet("/api/meetups")]
    [ProducesResponseType(typeof(ICollection<MeetupViewModel>), StatusCodes.Status200OK)]
    public Task<IActionResult> Perform(CancellationToken cancellationToken) =>
        mediator
            .SendAsync(new Request(), cancellationToken)
            .ToActionResult(Ok, InternalServerError);
}
