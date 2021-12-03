namespace Mitekat.Application.Features.GetMeetupById;

using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class GetMeetupByIdRequest : IRequest<MeetupViewModel>
{
    [FromRoute]
    public Guid MeetupId { get; set; }
}
