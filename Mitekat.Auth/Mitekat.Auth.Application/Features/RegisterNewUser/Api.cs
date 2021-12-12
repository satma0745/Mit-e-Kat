namespace Mitekat.Auth.Application.Features.RegisterNewUser;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
    private readonly IMapper mapper;

    public Action(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost("/api/auth/register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(mapper.Map<Request>(requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: Created,
                error => error switch
                {
                    Error.ConflictError => Conflict(),
                    _ => InternalServerError()
                });
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

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestBody, Request>();
}
