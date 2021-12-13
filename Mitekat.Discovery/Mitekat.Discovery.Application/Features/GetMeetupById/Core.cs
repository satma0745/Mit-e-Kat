namespace Mitekat.Discovery.Application.Features.GetMeetupById;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Discovery.Domain.Aggregates.Meetup;
using Mitekat.Seedwork.Features.Requesting;

internal record Request(Guid MeetupId) : RequestBase<MeetupViewModel>;

internal record MeetupViewModel(
    Guid Id,
    string Title,
    string Description,
    string Speaker,
    // TODO: Fix TimeSpan swagger example.
    TimeSpan Duration,
    DateTime StartTime,
    ICollection<Guid> SignedUpUserIds)
{
    // For AutoMapper
    private MeetupViewModel()
        : this(default, default, default, default, default, default, default)
    {
    }
}

internal class RequestHandler : RequestHandlerBase<Request, MeetupViewModel>
{
    private readonly IMeetupRepository repository;
    private readonly IMapper mapper;

    public RequestHandler(IMeetupRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public override async Task<Response<MeetupViewModel>> Handle(Request request, CancellationToken cancellationToken)
    {
        var meetup = await repository.GetSingle(request.MeetupId, cancellationToken);
        if (meetup is null)
        {
            return NotFoundFailure();
        }
            
        var viewModel = mapper.Map<MeetupViewModel>(meetup);
        return Success(viewModel);
    }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, MeetupViewModel>()
            .ForMember(
                model => model.SignedUpUserIds,
                options => options.MapFrom(meetup => meetup.SignedUpUsers.Select(user => user.Id)));
}
