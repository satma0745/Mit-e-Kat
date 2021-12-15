namespace Mitekat.Seedwork.Features.Requesting;

using System;
using MediatR;

public class RequestBase<TResource> : IRequest<Response<TResource>>
{
    public string AccessToken { get; set; }
    
    public virtual bool AuthenticationRequired => false;

    public virtual bool Authenticated => CurrentUser is not null;

    public virtual CurrentUserInfo CurrentUser { get; set; }
}

public record CurrentUserInfo(Guid Id);
