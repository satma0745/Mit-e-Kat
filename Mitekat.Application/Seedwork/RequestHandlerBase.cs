namespace Mitekat.Application.Seedwork;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mitekat.Model.Context;

internal abstract class RequestHandlerBase<TRequest, TResource> : IRequestHandler<TRequest, Response<TResource>>
    where TRequest : IRequest<Response<TResource>>
{
    protected MitekatContext Context { get; }
    
    protected IMapper Mapper { get; }

    protected RequestHandlerBase(MitekatContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public abstract Task<Response<TResource>> Handle(TRequest request, CancellationToken cancellationToken);

    protected static Response<TResource> Success(TResource resource) =>
        Response<TResource>.Success(resource);

    protected static Response<Unit> Success() =>
        Response<Unit>.Success(Unit.Value);

    protected static Response<TResource> NotFoundFailure() =>
        Response<TResource>.Failure(new Error.NotFoundError());
}
