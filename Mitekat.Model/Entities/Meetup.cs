namespace Mitekat.Model.Entities;

using System;

public class Meetup
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Speaker { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public DateTime StartTime { get; set; }
}
