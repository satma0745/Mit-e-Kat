namespace Mitekat.Application.Features;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mitekat.Application.Extensions;
using Mitekat.Application.Seedwork;
using Mitekat.Model.Context;
using Mitekat.Model.Entities;

public static class GetMeetupByIdFeature
{
    [Route("/api/meetups")]
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("{MeetupId:guid}")]
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
        public RequestHandler(MitekatContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override async Task<Response<ViewModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var meetup = await Context.Meetups
                .AsNoTracking()
                .SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId, cancellationToken);
            
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
