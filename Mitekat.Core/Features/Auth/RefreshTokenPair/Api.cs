namespace Mitekat.Core.Features.Auth.RefreshTokenPair;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Core.Abstractions.Helpers;
using Mitekat.Core.Seedwork.Action;
using Mitekat.Core.Seedwork.Features;

[Feature("Auth", "Refresh token pair")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPost("/api/auth/refresh")]
    [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Perform([FromBody] string refreshToken, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(new Request(refreshToken), cancellationToken)
            .ToActionResult(
                onSuccess: Ok,
                error => error switch
                {
                    Error.ConflictError => Conflict(),
                    _ => InternalServerError()
                });
}
