namespace Mitekat.Application.Features.GetAllMeetups;

using System.Collections.Generic;
using MediatR;

public class GetAllMeetupsRequest : IRequest<ICollection<MeetupViewModel>>
{
}
