﻿namespace Mitekat.Application.Features.Meetups;

using System;
using System.Collections.Generic;
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

[Feature("Meetups", "Get all meetups")]
public static class GetAllMeetupsFeature
{
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("/api/meetups")]
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
        // TODO: Consider defining Repository property in the RequestHandlerBase.
        private readonly IMeetupRepository repository;
        
        public RequestHandler(IMeetupRepository repository, IMapper mapper)
            : base(mapper) =>
            this.repository = repository;

        public override async Task<Response<ICollection<ViewModel>>> Handle(
            Request _,
            CancellationToken cancellationToken)
        {
            var meetups = await repository.GetAll(cancellationToken);
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