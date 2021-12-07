namespace Mitekat.Application.Features.Auth.RefreshTokenPair;

using System.Threading;
using System.Threading.Tasks;
using Mitekat.Application.Helpers;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.User;

internal record Request(string RefreshToken) : RequestBase<TokenPair>;

internal class RequestHandler : RequestHandlerBase<Request, TokenPair>
{
    private readonly AuthTokenHelper tokenHelper;
    private readonly IUserRepository repository;

    public RequestHandler(IUserRepository repository, AuthTokenHelper tokenHelper)
    {
        this.tokenHelper = tokenHelper;
        this.repository = repository;
    }

    public override async Task<Response<TokenPair>> Handle(Request request, CancellationToken cancellationToken)
    {
        var (userId, oldRefreshTokenId) = tokenHelper.ParseRefreshToken(request.RefreshToken);
        var user = await repository.GetSingle(userId, cancellationToken);

        var oldRefreshToken = user.GetRefreshToken(oldRefreshTokenId);
        if (oldRefreshToken is null)
        {
            return ConflictFailure();
        }

        var newRefreshToken = new RefreshToken(userId);
        user.ReplaceRefreshToken(oldRefreshToken, newRefreshToken);
        await repository.SaveChanges(cancellationToken);

        var tokenPair = tokenHelper.IssueTokenPair(newRefreshToken.UserId, newRefreshToken.TokenId);
        return Success(tokenPair);
    }
}
