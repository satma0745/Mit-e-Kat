namespace Mitekat.Application.Seedwork;

using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ActionBase : ControllerBase
{
    protected IMediator Mediator { get; }

    protected ActionBase(IMediator mediator) =>
        Mediator = mediator;

    protected IActionResult Created() =>
        StatusCode(StatusCodes.Status201Created);
    
    protected IActionResult InternalServerError() =>
        StatusCode(StatusCodes.Status500InternalServerError);
}
