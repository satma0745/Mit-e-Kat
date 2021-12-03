namespace Mitekat.Application.Features.RegisterNewMeetup;

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
public class RegisterNewMeetupAction : ControllerBase
{
    private readonly IMediator mediator;

    public RegisterNewMeetupAction(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Action(RegisterNewMeetupRequest request, CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }
}
