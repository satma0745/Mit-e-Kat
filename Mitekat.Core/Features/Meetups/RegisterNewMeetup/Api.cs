namespace Mitekat.Core.Features.Meetups.RegisterNewMeetup;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Core.Seedwork.Action;

public record RequestBody(string Title, string Description, string Speaker, int Duration, DateTime StartTime);

[Feature("Meetups", "Register a new meetup")]
public class Action : ActionBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public Action(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost("/api/meetups")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(mapper.Map<Request>(requestBody), cancellationToken)
            .ToActionResult(Created, InternalServerError);
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
