namespace Mitekat.Application.Features.Auth;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Seedwork;
using Mitekat.Model.Context;
using Mitekat.Model.Entities;

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
        public RequestHandler(MitekatContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override async Task<Response<Unit>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (await Context.Users.AnyAsync(user => user.Username == request.Properties.Username, cancellationToken))
            {
                return ConflictFailure();
            }

            var user = Mapper.Map<User>(request.Properties);
            user.Password = BCrypt.HashPassword(request.Properties.Password);
            
            Context.Users.Add(user);
            await Context.SaveChangesAsync(cancellationToken);

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

    internal class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<Request.UserProperties, User>();
    }
}
