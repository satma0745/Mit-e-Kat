﻿namespace Mitekat.Application.Features.RegisterNewMeetup;

using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class RegisterNewMeetupRequest : IRequest
{
    [FromBody]
    public MeetupProperties Properties { get; set; }
}

public class MeetupProperties
{
    public string Title { get; }
    
    public string Description { get; }
    
    public string Speaker { get; }
    
    public int Duration { get; }
    
    public DateTime StartTime { get; }

    public MeetupProperties(
        string title,
        string description,
        string speaker,
        int duration,
        DateTime startTime)
    {
        Title = title;
        Description = description;
        Speaker = speaker;
        Duration = duration;
        StartTime = startTime;
    }
}
