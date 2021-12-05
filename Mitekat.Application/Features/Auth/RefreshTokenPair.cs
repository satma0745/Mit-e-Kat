namespace Mitekat.Application.Features.Auth;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Helpers;
using Mitekat.Application.Seedwork;
using Mitekat.Model.Context;
using Mitekat.Model.Entities;

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
        
        public RequestHandler(MitekatContext context, IMapper mapper, AuthTokenHelper tokenHelper)
            : base(context, mapper) =>
            this.tokenHelper = tokenHelper;

        public override async Task<Response<ViewModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var oldRefreshTokenPayload = tokenHelper.ParseRefreshToken(request.RefreshToken);

            var oldRefreshToken = await Context.RefreshTokens
                .Where(refreshToken => refreshToken.TokenId == oldRefreshTokenPayload.TokenId)
                .SingleOrDefaultAsync(cancellationToken);
            if (oldRefreshToken is null)
            {
                return ConflictFailure();
            }
            Context.RefreshTokens.Remove(oldRefreshToken);

            var newRefreshToken = new RefreshToken
            {
                TokenId = Guid.NewGuid(),
                UserId = oldRefreshToken.UserId
            };
            Context.RefreshTokens.Add(newRefreshToken);
            await Context.SaveChangesAsync(cancellationToken);

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
