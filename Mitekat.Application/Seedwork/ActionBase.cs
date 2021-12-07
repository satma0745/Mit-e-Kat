namespace Mitekat.Application.Seedwork;

using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ActionBase : ControllerBase
{
    protected IActionResult Created() =>
        StatusCode(StatusCodes.Status201Created);
    
    protected IActionResult InternalServerError() =>
        StatusCode(StatusCodes.Status500InternalServerError);
}
