namespace Mitekat.Application.Features.GetMeetupById;

using System;
using System.Text.Json.Serialization;

internal class MeetupViewModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("title")]
    public string Title { get; init; }
    
    [JsonPropertyName("description")]
    public string Description { get; init; }
    
    [JsonPropertyName("speaker")]
    public string Speaker { get; init; }
    
    [JsonPropertyName("duration")]
    public TimeSpan Duration { get; init; }
    
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; init; }
}
