namespace Mitekat.Application.Seedwork;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

internal abstract class RequestHandlerBase<TRequest, TResource> : IRequestHandler<TRequest, Response<TResource>>
    where TRequest : IRequest<Response<TResource>>
{
    protected IMapper Mapper { get; }

    protected RequestHandlerBase(IMapper mapper) =>
        Mapper = mapper;

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
