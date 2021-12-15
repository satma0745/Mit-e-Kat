namespace Mitekat.Auth.Application.Features.Authenticate;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Auth.Application.Helpers.Tokens;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

public record RequestBody(string Username, string Password);

[Feature("Auth", "Authenticate user")]
public class Action : ActionBase
{
    private readonly IMediator mediator;
    
    public Action(IMediator mediator) =>
        this.mediator = mediator;
    
    [HttpPost("/api/auth/authenticate")]
    [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(CreateRequest(requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: Ok,
                error => error switch
                {
                    Error.NotFoundError => NotFound(),
                    Error.ConflictError => Conflict(),
                    _ => InternalServerError()
                });

    private static Request CreateRequest(RequestBody requestBody) =>
        new()
        {
            Username = requestBody.Username,
            Password = requestBody.Password
        };
}

internal class RequestBodyValidator : AbstractValidator<RequestBody>
{
    public RequestBodyValidator()
    {
        RuleFor(credentials => credentials.Username).NotEmpty();
        RuleFor(credentials => credentials.Password).NotEmpty();
    }
}
