namespace Mitekat.Discovery.Domain.Aggregates.Meetup;

using System;
using System.Collections.Generic;
using System.Linq;
using Mitekat.Discovery.Domain.Assertions;
using Mitekat.Discovery.Domain.Exceptions;
using Mitekat.Discovery.Domain.Seedwork;

public class Meetup : IAggregateRoot
{
    public Guid Id { get; }
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public string Speaker { get; private set; }
    
    public TimeSpan Duration { get; private set; }

    public DateTime StartTime { get; private set; }

    public IReadOnlyCollection<SignedUpUser> SignedUpUsers => signedUpUsers;
    private readonly List<SignedUpUser> signedUpUsers;

    public Meetup(Guid id, string title, string description, string speaker, TimeSpan duration, DateTime startTime)
    {
        Id = Assert.NotEmpty(id);
        Title = Assert.NotNullOrWhiteSpace(title);
        Description = Assert.NotNull(description);
        Speaker = Assert.NotNullOrWhiteSpace(speaker);
        Duration = Assert.InclusiveBetween(duration, TimeSpan.FromMinutes(30), TimeSpan.FromHours(16));
        StartTime = Assert.NotEmpty(startTime);

        signedUpUsers = new List<SignedUpUser>();
    }

    public Meetup(string title, string description, string speaker, TimeSpan duration, DateTime startTime)
        : this(id: Guid.NewGuid(), title, description, speaker, duration, startTime)
    {
    }

    public void SignUp(Guid userId)
    {
        if (signedUpUsers.Any(user => user.Id == userId))
        {
            throw new DomainException("User already signed up.");
        }
        
        var user = new SignedUpUser(userId);
        signedUpUsers.Add(user);
    }

    public void Update(string title, string description, string speaker, TimeSpan duration, DateTime startTime)
    {
        Title = Assert.NotNullOrWhiteSpace(title);
        Description = Assert.NotNull(description);
        Speaker = Assert.NotNullOrWhiteSpace(speaker);
        Duration = Assert.InclusiveBetween(duration, TimeSpan.FromMinutes(30), TimeSpan.FromHours(12));
        StartTime = Assert.NotEmpty(startTime);
    }
}
