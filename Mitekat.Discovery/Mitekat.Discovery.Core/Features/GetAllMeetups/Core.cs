namespace Mitekat.Discovery.Core.Features.GetAllMeetups;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mitekat.Discovery.Core.Abstraction.Repositories;
using Mitekat.Discovery.Core.Seedwork.Features;
using Mitekat.Discovery.Domain.Aggregates.Meetup;

internal record Request : RequestBase<ICollection<MeetupViewModel>>;

internal record MeetupViewModel(
    Guid Id,
    string Title,
    string Description,
    string Speaker,
    // TODO: Fix TimeSpan swagger example.
    TimeSpan Duration,
    DateTime StartTime);

internal class RequestHandler : RequestHandlerBase<Request, ICollection<MeetupViewModel>>
{
    private readonly IMeetupRepository repository;
    private readonly IMapper mapper;

    public RequestHandler(IMeetupRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public override async Task<Response<ICollection<MeetupViewModel>>> Handle(
        Request _,
        CancellationToken cancellationToken)
    {
        var meetups = await repository.GetAll(cancellationToken);
        var viewModels = mapper.Map<ICollection<MeetupViewModel>>(meetups);
        return Success(viewModels);
    }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, MeetupViewModel>();
}
