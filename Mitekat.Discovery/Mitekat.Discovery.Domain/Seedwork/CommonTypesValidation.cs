namespace Mitekat.Discovery.Domain.Seedwork;

using System;

internal class GuidValidationOptions
{
    public bool AllowEmpty { get; init; } = true;
}

internal class StringValidationOptions
{
    public bool AllowNull { get; init; } = true;

    public bool AllowEmpty { get; init; } = true;

    public int MinLength { get; init; } = 0;

    public int MaxLength { get; init; } = int.MaxValue;
}

internal class TimeSpanValidationOptions
{
    public TimeSpan Min { get; init; } = TimeSpan.MinValue;

    public TimeSpan Max { get; init; } = TimeSpan.MaxValue;
}

internal class DateTimeValidationOptions
{
    public bool AllowEmpty { get; init; } = true;
}

internal class CommonTypesValidation
{
    public static Guid ValidateGuid(Guid value, string propertyName, GuidValidationOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (!options.AllowEmpty && value == Guid.Empty)
        {
            throw DomainValidationException.Required(propertyName);
        }

        return value;
    }

    public static string ValidateString(string value, string propertyName, StringValidationOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (!options.AllowNull && value is null)
        {
            throw DomainValidationException.Required(propertyName);
        }
        if (!options.AllowEmpty && value == string.Empty)
        {
            throw DomainValidationException.Required(propertyName);
        }

        if (value?.Length < options.MinLength)
        {
            throw DomainValidationException.StringMinLength(propertyName, options.MinLength);
        }
        if (value?.Length > options.MaxLength)
        {
            throw DomainValidationException.StringMaxLength(propertyName, options.MaxLength);
        }

        return value;
    }

    public static TimeSpan ValidateTimeSpan(TimeSpan value, string propertyName, TimeSpanValidationOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (value < options.Min)
        {
            throw DomainValidationException.TimeSpanMin(propertyName, options.Min);
        }
        if (value > options.Max)
        {
            throw DomainValidationException.TimeSpanMax(propertyName, options.Max);
        }

        return value;
    }

    public static DateTime ValidateDateTime(DateTime value, string propertyName, DateTimeValidationOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (!options.AllowEmpty && value == default)
        {
            throw DomainValidationException.Required(propertyName);
        }

        return value;
    }
}
