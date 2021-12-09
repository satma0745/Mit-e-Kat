namespace Mitekat.Auth.Core.Features.Authenticate;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Auth.Core.Abstractions.Helpers;
using Mitekat.Auth.Core.Seedwork.Actions;
using Mitekat.Auth.Core.Seedwork.Features;

public record RequestBody(string Username, string Password);

[Feature("Auth", "Authenticate user")]
public class Action : ActionBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    
    public Action(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }
    
    [HttpPost("/api/auth/authenticate")]
    [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> Perform([FromBody] RequestBody requestBody, CancellationToken cancellationToken) =>
        mediator
            .SendAsync(mapper.Map<Request>(requestBody), cancellationToken)
            .ToActionResult(
                onSuccess: Ok,
                error => error switch
                {
                    Error.NotFoundError => NotFound(),
                    Error.ConflictError => Conflict(),
                    _ => InternalServerError()
                });
}

internal class RequestBodyValidator : AbstractValidator<RequestBody>
{
    public RequestBodyValidator()
    {
        RuleFor(credentials => credentials.Username).NotEmpty();
        RuleFor(credentials => credentials.Password).NotEmpty();
    }
}

internal class ApiMappingProfile : Profile
{
    public ApiMappingProfile() =>
        CreateMap<RequestBody, Request>();
}
