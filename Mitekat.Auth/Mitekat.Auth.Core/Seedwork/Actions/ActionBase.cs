namespace Mitekat.Auth.Core.Seedwork.Actions;

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
    protected string AccessToken =>
        Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim();
    
    protected IActionResult Created() =>
        StatusCode(StatusCodes.Status201Created);
    
    protected IActionResult InternalServerError() =>
        StatusCode(StatusCodes.Status500InternalServerError);
}
