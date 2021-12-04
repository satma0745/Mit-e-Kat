namespace Mitekat.Application.Features;

using System;
using System.Collections.Generic;
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

public static class GetAllMeetupsFeature
{
    [Route("/api/meetups")]
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ViewModel>), StatusCodes.Status200OK)]
        public Task<IActionResult> Perform(CancellationToken cancellationToken) =>
            Mediator
                .Send(new Request(), cancellationToken)
                .ToActionResult(Ok, InternalServerError);
    }
    
    internal class Request : RequestBase<ICollection<ViewModel>>
    {
    }

    internal record ViewModel(
        Guid Id,
        string Title,
        string Description,
        string Speaker,
        // TODO: Fix TimeSpan swagger example.
        TimeSpan Duration,
        DateTime StartTime
    );
    
    internal class RequestHandler : RequestHandlerBase<Request, ICollection<ViewModel>>
    {
        public RequestHandler(MitekatContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override async Task<Response<ICollection<ViewModel>>> Handle(
            Request _,
            CancellationToken cancellationToken)
        {
            var meetups = await Context.Meetups.AsNoTracking().ToListAsync(cancellationToken);
            var viewModels = Mapper.Map<ICollection<ViewModel>>(meetups);
            return Success(viewModels);
        }
    }
    
    internal class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<Meetup, ViewModel>();
    }
}
