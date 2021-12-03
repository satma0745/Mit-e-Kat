namespace Mitekat.Application.Features.RegisterNewMeetup;

using System;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class RegisterNewMeetupRequest : IRequest
{
    [FromBody]
    [JsonPropertyName("topic")]
    public string Title { get; }
    
    [FromBody]
    [JsonPropertyName("description")]
    public string Description { get; }
    
    [FromBody]
    [JsonPropertyName("speaker")]
    public string Speaker { get; }
    
    [FromBody]
    [JsonPropertyName("duration")]
    public int Duration { get; }
    
    [FromBody]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; }

    [JsonConstructor]
    public RegisterNewMeetupRequest(
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
