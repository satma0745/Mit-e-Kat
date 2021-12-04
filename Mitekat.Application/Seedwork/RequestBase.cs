namespace Mitekat.Application.Seedwork;

using MediatR;

public class RequestBase<TResource> : IRequest<Response<TResource>>
{
}
