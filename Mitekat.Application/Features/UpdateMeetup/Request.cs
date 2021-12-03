namespace Mitekat.Application.Features.UpdateMeetup;

using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class UpdateMeetupRequest : IRequest
{
    [FromRoute]
    public Guid MeetupId { get; set; }
    
    [FromBody]
    public EditableMeetupProperties Properties { get; set; }
}

public class EditableMeetupProperties
{
    public string Title { get; }
        
    public string Description { get; }
        
    public string Speaker { get; }
        
    public int Duration { get; }
        
    public DateTime StartTime { get; }

    public EditableMeetupProperties(string title, string description, string speaker, int duration, DateTime startTime)
    {
        Title = title;
        Description = description;
        Speaker = speaker;
        Duration = duration;
        StartTime = startTime;
    }
}
