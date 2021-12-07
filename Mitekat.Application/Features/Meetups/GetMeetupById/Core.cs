namespace Mitekat.Application.Features.Meetups.GetMeetupById;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.Meetup;

public record Request(Guid MeetupId) : RequestBase<MeetupViewModel>;

public record MeetupViewModel(
    Guid Id,
    string Title,
    string Description,
    string Speaker,
    // TODO: Fix TimeSpan swagger example.
    TimeSpan Duration,
    DateTime StartTime);

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
        CreateMap<Meetup, MeetupViewModel>();
}
