namespace Mitekat.Auth.Application.Features.RegisterNewUser;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

public record RequestBody(string Username, string Password, string DisplayName);

[Feature("Auth", "Register a new user")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPost("/api/auth/users/register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(CreateRequest(requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: _ => Created(),
                error => error switch
                {
                    Error.ConflictError => Conflict(),
                    _ => InternalServerError()
                });

    private static Request CreateRequest(RequestBody requestBody) =>
        new()
        {
            Username = requestBody.Username,
            Password = requestBody.Password,
            DisplayName = requestBody.DisplayName
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

        RuleFor(properties => properties.DisplayName)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(35);

        RuleFor(properties => properties.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20);
    }
}
