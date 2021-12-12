namespace Mitekat.Seedwork.Features.Actions;

using System;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ActionBase : ControllerBase
{
    protected virtual string AccessToken =>
        Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim();
    
    protected virtual IActionResult Created() =>
        StatusCode(StatusCodes.Status201Created);
    
    protected virtual IActionResult InternalServerError() =>
        StatusCode(StatusCodes.Status500InternalServerError);
}
