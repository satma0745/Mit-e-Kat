namespace Mitekat.Application.Features.Auth.Authenticate;

using System.Threading;
using System.Threading.Tasks;
using BCrypt.Net;
using Mitekat.Application.Helpers;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.User;

public record Request(string Username, string Password) : RequestBase<TokenPair>;

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
        var user = await repository.GetSingle(request.Username, cancellationToken);
        if (user is null)
        {
            return NotFoundFailure();
        }
        if (!BCrypt.Verify(request.Password, user.Password))
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
