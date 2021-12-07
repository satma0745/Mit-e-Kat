namespace Mitekat.Application.Seedwork;

using MediatR;

public record RequestBase<TResource> : IRequest<Response<TResource>>;
