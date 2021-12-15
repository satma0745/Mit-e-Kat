namespace Mitekat.Auth.Domain.Seedwork;

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
}
