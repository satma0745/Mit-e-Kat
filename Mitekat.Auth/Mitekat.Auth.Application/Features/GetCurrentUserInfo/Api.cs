namespace Mitekat.Auth.Application.Features.GetCurrentUserInfo;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

[Feature("Auth", "Get current user info")]
public class Action : ActionBase
{
    private readonly IMediator mediator;
    
    public Action(IMediator mediator) =>
        this.mediator = mediator;
    
    [HttpGet("/api/auth/users/self")]
    [ProducesResponseType(typeof(ViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> Perform(CancellationToken cancellationToken) =>
        mediator
            .SendAsync(new Request(AccessToken), cancellationToken)
            .ToActionResult(
                onSuccess: Ok,
                error => error switch
                {
                    Error.UnauthorizedError => Unauthorized(),
                    _ => InternalServerError()
                });
}
