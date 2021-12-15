namespace Mitekat.Auth.Application.Features.RefreshTokenPair;

using System.Threading;
using System.Threading.Tasks;
using Mitekat.Auth.Application.Helpers.Tokens;
using Mitekat.Auth.Application.Persistence.Repositories;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<TokenPair>
{
    public string RefreshToken { get; }

    public Request(string refreshToken) =>
        RefreshToken = refreshToken;
}

internal class RequestHandler : RequestHandlerBase<Request, TokenPair>
{
    private readonly ITokenHelper tokenHelper;
    private readonly IUserRepository repository;

    public RequestHandler(IUserRepository repository, ITokenHelper tokenHelper)
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
