namespace Mitekat.Application.Features.Auth;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.User;

[Feature("Auth", "Register a new user")]
public static class RegisterNewUserFeature
{
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("/api/auth/register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public Task<IActionResult> Perform(Request request, CancellationToken cancellationToken) =>
            Mediator
                .Send(request, cancellationToken)
                .ToActionResult(
                    onSuccess: Created,
                    error => error switch
                    {
                        Error.ConflictError => Conflict(),
                        _ => InternalServerError()
                    });
    }

    public class Request : RequestBase<Unit>
    {
        [FromBody]
        public UserProperties Properties { get; set; }

        public record UserProperties(string Username, string DisplayName, string Password);
    }

    internal class RequestHandler : RequestHandlerBase<Request, Unit>
    {
        private readonly IUserRepository repository;

        public RequestHandler(IUserRepository repository, IMapper mapper)
            : base(mapper) =>
            this.repository = repository;

        public override async Task<Response<Unit>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (await repository.UsernameTaken(request.Properties.Username, cancellationToken))
            {
                return ConflictFailure();
            }

            // TODO: Figure out a better way to instantiate an aggregate root from a request.
            var user = new User(
                request.Properties.Username,
                request.Properties.DisplayName,
                BCrypt.HashPassword(request.Properties.Password));
            repository.Add(user);
            
            await repository.SaveChanges(cancellationToken);
            return Success();
        }
    }

    internal class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator() =>
            RuleFor(request => request.Properties)
                .NotNull()
                .SetValidator(new PropertiesValidator());

        private class PropertiesValidator : AbstractValidator<Request.UserProperties>
        {
            // TODO: Consider creating a separate validator for each field.
            public PropertiesValidator()
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
    }
}
