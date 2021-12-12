namespace Mitekat.Seedwork.Features.Requesting;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public abstract class RequestHandlerBase<TRequest, TResource> : IRequestHandler<TRequest, Response<TResource>>
    where TRequest : IRequest<Response<TResource>>
{
    public abstract Task<Response<TResource>> Handle(TRequest request, CancellationToken cancellationToken);

    protected static Response<TResource> Success(TResource resource) =>
        Response<TResource>.Success(resource);

    protected static Response<Unit> Success() =>
        Response<Unit>.Success(Unit.Value);

    protected static Response<TResource> NotFoundFailure() =>
        Response<TResource>.Failure(new Error.NotFoundError());

    protected static Response<TResource> ConflictFailure() =>
        Response<TResource>.Failure(new Error.ConflictError());
}
