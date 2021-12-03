namespace Mitekat.Application.Features.GetMeetupById;

using System;
using MediatR;

internal class GetMeetupByIdRequest : IRequest<MeetupViewModel>
{
    public Guid MeetupId { get; }

    public GetMeetupByIdRequest(Guid meetupId) =>
        MeetupId = meetupId;
}
