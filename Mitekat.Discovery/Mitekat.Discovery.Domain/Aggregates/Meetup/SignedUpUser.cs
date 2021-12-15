namespace Mitekat.Discovery.Domain.Aggregates.Meetup;

using System;

public class SignedUpUser
{
    public Guid Id { get; }

    public SignedUpUser(Guid id) =>
        Id = MeetupValidation.EnsureValidUserId(id);
}
