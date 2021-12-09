namespace Mitekat.Discovery.Core.Seedwork.Behaviors;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Discovery.Core.Abstraction.Helpers;
using Mitekat.Discovery.Core.Seedwork.Features;

internal class AuthenticationBehavior<TAnyRequest, TResponse> : PipelineBehaviorBase<TAnyRequest, TResponse>
{
    private readonly ITokenHelper tokenHelper;

    public AuthenticationBehavior(ITokenHelper tokenHelper) =>
        this.tokenHelper = tokenHelper;

    protected override async Task<Response<TResult>> HandleAsync<TRequest, TResult>(
        TRequest request,
        RequestHandlerDelegate<Response<TResult>> next,
        CancellationToken cancellationToken)
    {
        var accessTokenPayload = tokenHelper.ParseAccessToken(request.AccessToken);
        
        if (accessTokenPayload is not null)
        {
            var userInfo = new CurrentUserInfo(accessTokenPayload.UserId);
            request.CurrentUser = userInfo;
        }
        else if (request.AuthenticationRequired)
        {
            var error = new Error.UnauthorizedError();
            return Response<TResult>.Failure(error);
        }

        return await next();
    }
}
