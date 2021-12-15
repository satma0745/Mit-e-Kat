namespace Mitekat.Auth.Application.Features.UpdateCredentials;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Auth.Application.Helpers.Passwords;
using Mitekat.Auth.Application.Persistence.Repositories;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<Unit>
{
    public override bool AuthenticationRequired => true;

    public string Username { get; init; }
    
    public string Password { get; init; }
}

internal class RequestHandler : RequestHandlerBase<Request, Unit>
{
    private readonly IUserRepository repository;
    private readonly IPasswordHelper passwordHelper;

    public RequestHandler(IUserRepository repository, IPasswordHelper passwordHelper)
    {
        this.repository = repository;
        this.passwordHelper = passwordHelper;
    }

    public override async Task<Response<Unit>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await repository.GetSingle(request.CurrentUser.Id, cancellationToken);

        var newUsername = request.Username;
        var newPassword = passwordHelper.Hash(request.Password);
        user.ChangeCredentials(newUsername, newPassword);
        
        await repository.SaveChanges(cancellationToken);
        return Success();
    }
}
