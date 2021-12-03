namespace Mitekat.Application.Features.UpdateMeetup;

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
public class UpdateMeetupAction : ControllerBase
{
    private readonly IMediator mediator;

    public UpdateMeetupAction(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPut("{MeetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMeetup(UpdateMeetupRequest request, CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return Ok();
    }
}
