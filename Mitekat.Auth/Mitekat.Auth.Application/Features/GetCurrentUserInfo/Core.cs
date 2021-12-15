namespace Mitekat.Auth.Application.Features.GetCurrentUserInfo;

using System;
using System.Threading;
using System.Threading.Tasks;
using Mitekat.Auth.Application.Persistence.Repositories;
using Mitekat.Auth.Domain.Aggregates.User;
using Mitekat.Seedwork.Features.Requesting;

internal class Request : RequestBase<ViewModel>
{
    public override bool AuthenticationRequired => true;

    public Request(string accessToken) =>
        AccessToken = accessToken;
}

internal class ViewModel
{
    public Guid Id { get; init; }
    
    public string Username { get; init; }

    public string DisplayName { get; init; }
}

internal class RequestHandler : RequestHandlerBase<Request, ViewModel>
{
    private readonly IUserRepository repository;

    public RequestHandler(IUserRepository repository) =>
        this.repository = repository;

    public override async Task<Response<ViewModel>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await repository.GetSingle(request.CurrentUser.Id, cancellationToken);
        if (user is null)
        {
            return NotFoundFailure();
        }

        var viewModel = ToViewModel(user);
        return Success(viewModel);
    }

    private static ViewModel ToViewModel(User user) =>
        new()
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName
        };
}
