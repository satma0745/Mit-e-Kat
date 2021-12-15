namespace Mitekat.Auth.Application.Features.UpdateCredentials;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

public record RequestBody(string Username, string Password);

[Feature("Auth", "Update user credentials")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPut("/api/auth/users/self/credentials")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(CreateRequest(requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: _ => NoContent(),
                error => error switch
                {
                    Error.UnauthorizedError => Unauthorized(),
                    _ => InternalServerError()
                });

    private Request CreateRequest(RequestBody requestBody) =>
        new()
        {
            Username = requestBody.Username,
            Password = requestBody.Password,
            AccessToken = AccessToken
        };
}

internal class RequestBodyValidator : AbstractValidator<RequestBody>
{
    // TODO: Consider creating a separate validator for each field.
    public RequestBodyValidator()
    {
        RuleFor(properties => properties.Username)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);

        RuleFor(properties => properties.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);
    }
}
