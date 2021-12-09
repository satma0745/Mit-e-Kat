namespace Mitekat.Core.Features.Auth.Authenticate;

using System.Threading;
using System.Threading.Tasks;
using Mitekat.Core.Abstractions.Helpers;
using Mitekat.Core.Abstractions.Repositories;
using Mitekat.Core.Seedwork.Features;
using Mitekat.Domain.Aggregates.User;

public record Request(string Username, string Password) : RequestBase<TokenPair>;

internal class RequestHandler : RequestHandlerBase<Request, TokenPair>
{
    private readonly ITokenHelper tokenHelper;
    private readonly IPasswordHelper passwordHelper;
    private readonly IUserRepository repository;

    public RequestHandler(IUserRepository repository, IPasswordHelper passwordHelper, ITokenHelper tokenHelper)
    {
        this.tokenHelper = tokenHelper;
        this.passwordHelper = passwordHelper;
        this.repository = repository;
    }

    public override async Task<Response<TokenPair>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await repository.GetSingle(request.Username, cancellationToken);
        if (user is null)
        {
            return NotFoundFailure();
        }
        if (!passwordHelper.Match(request.Password, user.Password))
        {
            return ConflictFailure();
        }

        var refreshToken = new RefreshToken(user.Id);
        user.AddRefreshToken(refreshToken);
        await repository.SaveChanges(cancellationToken);

        var tokenPair = tokenHelper.IssueTokenPair(user.Id, refreshToken.TokenId);
        return Success(tokenPair);
    }
}
