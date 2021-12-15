namespace Mitekat.Discovery.Application.Features.UpdateMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Actions;
using Mitekat.Seedwork.Features.Requesting;

public record RequestBody(string Title, string Description, string Speaker, int Duration, DateTime StartTime);

[Feature("Meetups", "Update meetup")]
public class Action : ActionBase
{
    private readonly IMediator mediator;

    public Action(IMediator mediator) =>
        this.mediator = mediator;

    [HttpPut("/api/meetups/{meetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> Perform([FromRoute] Guid meetupId, [FromBody] RequestBody requestBody,
        CancellationToken cancellationToken) =>
        mediator
            .SendAsync(CreateRequest(meetupId, requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: _ => NoContent(),
                error => error switch
                {
                    Error.NotFoundError => NotFound(),
                    _ => InternalServerError()
                });

    private static Request CreateRequest(Guid meetupId, RequestBody requestBody) =>
        new()
        {
            MeetupId = meetupId,
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
