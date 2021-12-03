namespace Mitekat.Application.Features.GetMeetupById;

using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/meetups")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class GetMeetupByIdAction : ControllerBase
{
    private readonly IMediator mediator;

    public GetMeetupByIdAction(IMediator mediator) =>
        this.mediator = mediator;

    [HttpGet("{meetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Action([FromRoute] Guid meetupId, CancellationToken cancellationToken)
    {
        var meetup = await mediator.Send(new GetMeetupByIdRequest(meetupId), cancellationToken);
        return Ok(meetup);
    }
}
