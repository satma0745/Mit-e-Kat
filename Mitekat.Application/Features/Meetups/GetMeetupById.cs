namespace Mitekat.Application.Features.Meetups;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Seedwork;
using Mitekat.Domain.Aggregates.Meetup;

[Feature("Meetups", "Get meetup by id")]
public static class GetMeetupByIdFeature
{
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("/api/meetups/{MeetupId:guid}")]
        [ProducesResponseType(typeof(ViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Perform(Request request, CancellationToken cancellationToken) =>
            Mediator
                .Send(request, cancellationToken)
                .ToActionResult(
                    onSuccess: Ok,
                    error => error switch
                    {
                        Error.NotFoundError => NotFound(),
                        _ => InternalServerError()
                    });
    }
    
    public class Request : RequestBase<ViewModel>
    {
        [FromRoute]
        public Guid MeetupId { get; set; }
    }
    
    public record ViewModel(
        Guid Id,
        string Title,
        string Description,
        string Speaker,
        // TODO: Fix TimeSpan swagger example.
        TimeSpan Duration,
        DateTime StartTime
    );

    internal class RequestHandler : RequestHandlerBase<Request, ViewModel>
    {
        private readonly IMeetupRepository repository;

        public RequestHandler(IMeetupRepository repository, IMapper mapper)
            : base(mapper) =>
            this.repository = repository;

        public override async Task<Response<ViewModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var meetup = await repository.GetSingle(request.MeetupId, cancellationToken);
            if (meetup is null)
            {
                return NotFoundFailure();
            }
            
            var viewModel = Mapper.Map<ViewModel>(meetup);
            return Success(viewModel);
        }
    }
    
    internal class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<Meetup, ViewModel>();
    }
}
