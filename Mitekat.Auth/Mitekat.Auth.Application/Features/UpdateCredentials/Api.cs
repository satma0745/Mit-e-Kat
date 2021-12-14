namespace Mitekat.Auth.Application.Features.UpdateCredentials;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
    private readonly IMapper mapper;

    public Action(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPut("/api/auth/users/self/credentials")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(() =>
            {
                var request = new Request(AccessToken);
                return mapper.Map(requestBody, request);
            }, cancellationToken)
            .ToActionResult(
                onSuccess: _ => Ok(),
                error => error switch
                {
                    Error.UnauthorizedError => Unauthorized(),
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
