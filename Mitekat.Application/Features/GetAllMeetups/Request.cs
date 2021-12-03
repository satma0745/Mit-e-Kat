namespace Mitekat.Application.Features.GetAllMeetups;

using System.Collections.Generic;
using MediatR;

internal class GetAllMeetupsRequest : IRequest<ICollection<MeetupViewModel>>
{
}
