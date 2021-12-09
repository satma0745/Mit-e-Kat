namespace Mitekat.Auth.Core.Features.GetCurrentUserInfo;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mitekat.Auth.Core.Abstractions.Repositories;
using Mitekat.Auth.Core.Seedwork.Features;
using Mitekat.Auth.Domain.Aggregates.User;

internal record Request : RequestBase<UserInfo>
{
    public override bool AuthenticationRequired => true;

    public Request(string accessToken) =>
        AccessToken = accessToken;
}

internal record UserInfo(Guid Id, string Username, string DisplayName);

internal class RequestHandler : RequestHandlerBase<Request, UserInfo>
{
    private readonly IUserRepository repository;
    private readonly IMapper mapper;

    public RequestHandler(IUserRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public override async Task<Response<UserInfo>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await repository.GetSingle(request.CurrentUser.Id, cancellationToken);
        if (user is null)
        {
            return NotFoundFailure();
        }

        var userInfo = mapper.Map<UserInfo>(user);
        return Success(userInfo);
    }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<User, UserInfo>();
}
