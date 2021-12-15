namespace Mitekat.Auth.Application.Features.GetCurrentUserInfo;

using System;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Auth.Application.Persistence.Repositories;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<UserInfoViewModel>
{
    public override bool AuthenticationRequired => true;

    public Request(string accessToken) =>
        AccessToken = accessToken;
}

internal class UserInfoViewModel
{
    public Guid Id { get; init; }
    
    public string Username { get; init; }

    public string DisplayName { get; init; }
}

internal class RequestHandler : RequestHandlerBase<Request, UserInfoViewModel>
{
    private readonly IUserRepository repository;

    public RequestHandler(IUserRepository repository) =>
        this.repository = repository;

    public override async Task<Response<UserInfoViewModel>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await repository.GetSingle(request.CurrentUser.Id, cancellationToken);
        if (user is null)
        {
            return NotFoundFailure();
        }

        var userInfo = ToViewModel(user);
        return Success(userInfo);
    }

    private static UserInfoViewModel ToViewModel(User user) =>
        new()
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName
        };
}
