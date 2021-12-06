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
using Mitekat.Application.Helpers;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.User;

[Feature("Auth", "Authenticate user")]
public static class Authenticate
{
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("/api/auth/authenticate")]
        [ProducesResponseType(typeof(ViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public Task<IActionResult> Perform(Request request, CancellationToken cancellationToken) =>
            Mediator
                .Send(request, cancellationToken)
                .ToActionResult(
                    onSuccess: Ok,
                    error => error switch
                    {
                        Error.NotFoundError => NotFound(),
                        Error.ConflictError => Conflict(),
                        _ => InternalServerError()
                    });
    }
    
    public class Request : RequestBase<ViewModel>
    {
        [FromBody]
        public UserCredentials Credentials { get; set; }

        public record UserCredentials(string Username, string Password);
    }

    public record ViewModel(string AccessToken, string RefreshToken);

    internal class RequestHandler : RequestHandlerBase<Request, ViewModel>
    {
        private readonly AuthTokenHelper tokenHelper;
        private readonly IUserRepository repository;

        public RequestHandler(IUserRepository repository, IMapper mapper, AuthTokenHelper tokenHelper)
            : base(mapper)
        {
            this.tokenHelper = tokenHelper;
            this.repository = repository;
        }

        public override async Task<Response<ViewModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await repository.GetSingle(request.Credentials.Username, cancellationToken);
            if (user is null)
            {
                return NotFoundFailure();
            }
            if (!BCrypt.Verify(request.Credentials.Password, user.Password))
            {
                return ConflictFailure();
            }

            var refreshToken = new RefreshToken(user.Id);
            user.AddRefreshToken(refreshToken);
            await repository.SaveChanges(cancellationToken);

            var tokenPair = tokenHelper.IssueTokenPair(user.Id, refreshToken.TokenId);
            var viewModel = Mapper.Map<ViewModel>(tokenPair);
            return Success(viewModel);
        }
    }

    internal class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator() =>
            RuleFor(request => request.Credentials)
                .NotNull()
                .SetValidator(new CredentialsValidator());
        
        private class CredentialsValidator : AbstractValidator<Request.UserCredentials>
        {
            public CredentialsValidator()
            {
                RuleFor(credentials => credentials.Username).NotEmpty();
                RuleFor(credentials => credentials.Password).NotEmpty();
            }
        }
    }

    internal class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<AuthTokenHelper.TokenPair, ViewModel>();
    }
}
