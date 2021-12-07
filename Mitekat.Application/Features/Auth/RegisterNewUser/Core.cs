namespace Mitekat.Application.Features.Auth.RegisterNewUser;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BCrypt.Net;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.User;

internal record Request(string Username, string DisplayName, string Password) : RequestBase<Unit>;

internal class RequestHandler : RequestHandlerBase<Request, Unit>
{
    private readonly IUserRepository repository;

    public RequestHandler(IUserRepository repository) =>
        this.repository = repository;

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
            BCrypt.HashPassword(request.Password));
        repository.Add(user);
            
        await repository.SaveChanges(cancellationToken);
        return Success();
    }
}
