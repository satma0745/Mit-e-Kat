namespace Mitekat.Auth.Domain.Aggregates.User;

using System;
using Mitekat.Auth.Domain.Seedwork;

internal static class UserValidation
{
    public static Guid EnsureValidUserId(Guid id) =>
        CommonTypesValidation.ValidateGuid(id, "User ID", new GuidValidationOptions {AllowEmpty = false});

    public static string EnsureValidUserUsername(string username, string propertyName = "User username")
    {
        var options = new StringValidationOptions
        {
            AllowNull = false,
            MinLength = 6,
            MaxLength = 20
        };

        return CommonTypesValidation.ValidateString(username, propertyName, options);
    }

    public static string EnsureValidUserPassword(string password, string propertyName = "User password")
    {
        var options = new StringValidationOptions
        {
            AllowNull = false,
            AllowEmpty = false
        };
        
        return CommonTypesValidation.ValidateString(password, propertyName, options);
    }

    public static string EnsureValidUserDisplayName(string displayName, string propertyName = "User display name")
    {
        var options = new StringValidationOptions
        {
            AllowNull = false,
            MinLength = 6,
            MaxLength = 35
        };

        return CommonTypesValidation.ValidateString(displayName, propertyName, options);
    }
}
