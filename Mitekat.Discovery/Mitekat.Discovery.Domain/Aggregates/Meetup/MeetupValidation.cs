namespace Mitekat.Discovery.Domain.Aggregates.Meetup;

using System;
using Mitekat.Discovery.Domain.Seedwork;

internal static class MeetupValidation
{
    public static Guid EnsureValidMeetupId(Guid id, string parameterName = "Meetup ID") =>
        CommonTypesValidation.ValidateGuid(id, parameterName, new GuidValidationOptions {AllowEmpty = false});

    public static Guid EnsureValidUserId(Guid id, string parameterName = "User ID") =>
        CommonTypesValidation.ValidateGuid(id, parameterName, new GuidValidationOptions {AllowEmpty = false});

    public static string EnsureValidMeetupTitle(string title, string propertyName = "Meetup title")
    {
        var options = new StringValidationOptions
        {
            AllowNull = false,
            MinLength = 6,
            MaxLength = 100
        };

        return CommonTypesValidation.ValidateString(title, propertyName, options);
    }

    public static string EnsureValidMeetupDescription(string description, string propertyName = "Meetup description")
    {
        var options = new StringValidationOptions
        {
            AllowNull = false,
            MaxLength = 2500
        };

        return CommonTypesValidation.ValidateString(description, propertyName, options);
    }

    public static string EnsureValidMeetupSpeaker(string speaker, string propertyName = "Meetup speaker")
    {
        var options = new StringValidationOptions
        {
            AllowNull = false,
            AllowEmpty = false,
            MaxLength = 50
        };

        return CommonTypesValidation.ValidateString(speaker, propertyName, options);
    }

    public static TimeSpan EnsureValidMeetupDuration(TimeSpan duration, string propertyName = "Meetup duration")
    {
        var options = new TimeSpanValidationOptions
        {
            Min = TimeSpan.FromMinutes(30),
            Max = TimeSpan.FromHours(16)
        };

        return CommonTypesValidation.ValidateTimeSpan(duration, propertyName, options);
    }

    public static DateTime EnsureValidMeetupStartTime(DateTime startTime, string propertyName = "Meetup start time")
    {
        var options = new DateTimeValidationOptions
        {
            AllowEmpty = false
        };

        return CommonTypesValidation.ValidateDateTime(startTime, propertyName, options);
    }
}
