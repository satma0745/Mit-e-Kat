namespace Mitekat.Auth.Application.Features.RegisterNewUser;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Auth.Application.Helpers.Passwords;
using Mitekat.Auth.Application.Persistence.Repositories;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Features.Requesting;

internal record Request(string Username, string DisplayName, string Password) : RequestBase<Unit>;

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
        if (await repository.UsernameTaken(request.Username, cancellationToken))
        {
            return ConflictFailure();
        }

        // TODO: Figure out a better way to instantiate an aggregate root from a request.
        var user = new User(
            request.Username,
            request.DisplayName,
            passwordHelper.Hash(request.Password));
        repository.Add(user);
            
        await repository.SaveChanges(cancellationToken);
        return Success();
    }
}
