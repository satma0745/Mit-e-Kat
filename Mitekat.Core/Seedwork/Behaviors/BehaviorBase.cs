namespace Mitekat.Core.Seedwork.Behaviors;

using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mitekat.Core.Seedwork.Features;

internal abstract class PipelineBehaviorBase<TAnyRequest, TResponse> : IPipelineBehavior<TAnyRequest, TResponse>
{
    public Task<TResponse> Handle(
        TAnyRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var isSupportedRequestType = typeof(TAnyRequest).IsSubclassOfGeneric(typeof(RequestBase<>));
        var isSupportedResponseType = typeof(TResponse).GetGenericTypeDefinition() == typeof(Response<>);
        if (!isSupportedRequestType || !isSupportedResponseType)
        {
            return next();
        }

        const string methodName = nameof(HandleAsync);
        const BindingFlags methodType = BindingFlags.Instance | BindingFlags.NonPublic;

        var requestType = typeof(TAnyRequest);
        var resourceType = typeof(TResponse).GenericTypeArguments.Single();

        var handleAsyncMethod = GetType()
            .GetMethod(methodName, methodType)!
            .MakeGenericMethod(requestType, resourceType);

        var arguments = new object[] {request, next, cancellationToken};
        return (Task<TResponse>) handleAsyncMethod.Invoke(this, arguments);
    }

    protected abstract Task<Response<TResult>> HandleAsync<TRequest, TResult>(
        TRequest request,
        RequestHandlerDelegate<Response<TResult>> next,
        CancellationToken cancellationToken)
        where TRequest : RequestBase<TResult>;
}
