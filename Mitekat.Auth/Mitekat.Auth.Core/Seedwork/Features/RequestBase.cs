namespace Mitekat.Auth.Core.Seedwork.Features;

using System;
using MediatR;

public record RequestBase<TResource> : IRequest<Response<TResource>>
{
    public string AccessToken { get; set; }
    
    public virtual bool AuthenticationRequired => false;

    public bool Authenticated => CurrentUser is not null;

    public CurrentUserInfo CurrentUser { get; set; }
}

public record CurrentUserInfo(Guid Id);
