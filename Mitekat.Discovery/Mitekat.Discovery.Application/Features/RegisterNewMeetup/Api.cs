namespace Mitekat.Discovery.Application.Features.RegisterNewMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;

public record RequestBody(string Title, string Description, string Speaker, int Duration, DateTime StartTime);

[Feature("Meetups", "Register a new meetup")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPost("/api/meetups")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(CreateRequest(requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: _ => Created(),
                onFailure: _ => InternalServerError());

    private static Request CreateRequest(RequestBody requestBody) =>
        new()
        {
            Title = requestBody.Title,
            Description = requestBody.Description,
            Speaker = requestBody.Speaker,
            Duration = requestBody.Duration,
            StartTime = requestBody.StartTime
        };
}

internal class RequestBodyValidator : AbstractValidator<RequestBody>
{
    // TODO: Consider creating a separate validator for each field.
    public RequestBodyValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);
        
        RuleFor(request => request.Description)
            .NotNull()
            .MaximumLength(2500);
        
        RuleFor(request => request.Speaker)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(request => request.Duration)
            .NotEmpty()
            .GreaterThanOrEqualTo(60 / 2)
            .LessThanOrEqualTo(16 * 60);
        
        RuleFor(request => request.StartTime).NotEmpty();
    }
}
