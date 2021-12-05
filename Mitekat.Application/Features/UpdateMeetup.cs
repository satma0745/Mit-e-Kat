namespace Mitekat.Application.Features;

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mitekat.Application.Conventions;
using Mitekat.Application.Extensions;
using Mitekat.Application.Seedwork;
using Mitekat.Model.Context;
using Mitekat.Model.Entities;

[Feature("Meetups", "Update meetup")]
public static class UpdateMeetupFeature
{
    public class Action : ActionBase
    {
        public Action(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPut("/api/meetups/{MeetupId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> Perform(Request request, CancellationToken cancellationToken) =>
            Mediator
                .Send(request, cancellationToken)
                .ToActionResult(
                    onSuccess: NoContent,
                    error => error switch
                    {
                        Error.NotFoundError => NotFound(),
                        _ => InternalServerError()
                    });
    }
    
    public class Request : RequestBase<Unit>
    {
        [FromRoute]
        public Guid MeetupId { get; set; }
    
        [FromBody]
        public MeetupProperties Properties { get; set; }

        public record MeetupProperties(
            string Title,
            string Description,
            string Speaker,
            int Duration,
            DateTime StartTime);
    }
    
    internal class RequestHandler : RequestHandlerBase<Request, Unit>
    {
        public RequestHandler(MitekatContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override async Task<Response<Unit>> Handle(Request request, CancellationToken cancellationToken)
        {
            var meetup = await Context.Meetups
                .SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId, cancellationToken);
            if (meetup is null)
            {
                return NotFoundFailure();
            }
        
            Mapper.Map(request.Properties, meetup);
            await Context.SaveChangesAsync(cancellationToken);

            return Success();
        }
    }
    
    internal class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.MeetupId).NotEmpty();

            RuleFor(request => request.Properties)
                .NotNull()
                .SetValidator(new PropertiesValidator());
        }
        
        private class PropertiesValidator : AbstractValidator<Request.MeetupProperties>
        {
            // TODO: Consider creating a separate validator for each field.
            public PropertiesValidator()
            {
                RuleFor(request => request.Title)
                    .NotEmpty()
                    .MinimumLength(6)
                    .MaximumLength(100);
        
                RuleFor(request => request.Description)
                    .NotNull()
                    .MaximumLength(2500);
        
                RuleFor(request => request.Speaker)
                    .NotEmpty()
                    .MaximumLength(50);
        
                RuleFor(request => request.Duration)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(60 / 2)
                    .LessThanOrEqualTo(16 * 60);
        
                RuleFor(request => request.StartTime).NotEmpty();
            }
        }
    }
    
    internal class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<Request.MeetupProperties, Meetup>()
                .ForMember(
                    meetup => meetup.Duration,
                    options => options.MapFrom(properties => TimeSpan.FromMinutes(properties.Duration)));
    }
}
