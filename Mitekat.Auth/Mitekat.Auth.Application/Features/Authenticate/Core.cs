namespace Mitekat.Auth.Application.Features.Authenticate;

using System.Threading;
using System.Threading.Tasks;
using Mitekat.Auth.Application.Helpers.Passwords;
using Mitekat.Auth.Application.Helpers.Tokens;
using Mitekat.Auth.Application.Persistence.Repositories;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<TokenPair>
{
    public string Username { get; init; }
    
    public string Password { get; init; }
}

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
