namespace Mitekat.Application.Features.Auth;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Helpers;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.User;

[Feature("Auth", "Refresh token pair")]
public static class RefreshTokenPair
{
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("/api/auth/refresh")]
        [ProducesResponseType(typeof(ViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public Task<IActionResult> Perform(Request request, CancellationToken cancellationToken) =>
            Mediator
                .Send(request, cancellationToken)
                .ToActionResult(
                    onSuccess: Ok,
                    error => error switch
                    {
                        Error.ConflictError => Conflict(),
                        _ => InternalServerError()
                    });
    }
    
    public class Request : RequestBase<ViewModel>
    {
        [FromBody]
        public string RefreshToken { get; set; }
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
            var oldRefreshTokenPayload = tokenHelper.ParseRefreshToken(request.RefreshToken);
            var user = await repository.GetSingle(oldRefreshTokenPayload.UserId, cancellationToken);

            var oldRefreshToken = user.GetRefreshToken(oldRefreshTokenPayload.TokenId);
            if (oldRefreshToken is null)
            {
                return ConflictFailure();
            }

            var newRefreshToken = new RefreshToken(user.Id);
            user.ReplaceRefreshToken(oldRefreshToken, newRefreshToken);
            await repository.SaveChanges(cancellationToken);

            var tokenPair = tokenHelper.IssueTokenPair(newRefreshToken.UserId, newRefreshToken.TokenId);
            var viewModel = Mapper.Map<ViewModel>(tokenPair);
            return Success(viewModel);
        }
    }

    internal class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator() =>
            RuleFor(request => request.RefreshToken).NotEmpty();
    }

    internal class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<AuthTokenHelper.TokenPair, ViewModel>();
    }
}
