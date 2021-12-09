namespace Mitekat.Core.Features.Meetups.UpdateMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Core.Seedwork.Action;
using Mitekat.Core.Seedwork.Features;

public record RequestBody(string Title, string Description, string Speaker, int Duration, DateTime StartTime);

[Feature("Meetups", "Update meetup")]
public class Action : ActionBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public Action(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPut("/api/meetups/{meetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> Perform([FromRoute] Guid meetupId, [FromBody] RequestBody requestBody,
        CancellationToken cancellationToken) =>
        mediator
            .SendAsync(() =>
            {
                var request = new Request(meetupId);
                mapper.Map(requestBody, request);
                return request;
            }, cancellationToken)
            .ToActionResult(
                onSuccess: NoContent,
                error => error switch
                {
                    Error.NotFoundError => NotFound(),
                    _ => InternalServerError()
                });
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

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestBody, Request>();
}
