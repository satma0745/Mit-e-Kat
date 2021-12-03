namespace Mitekat.Application.Features.GetAllMeetups;

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
public class GetAllMeetupsAction : ControllerBase
{
    private readonly IMediator mediator;

    public GetAllMeetupsAction(IMediator mediator) =>
        this.mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Action(CancellationToken cancellationToken)
    {
        var meetups = await mediator.Send(new GetAllMeetupsRequest(), cancellationToken);
        return Ok(meetups);
    }
}
