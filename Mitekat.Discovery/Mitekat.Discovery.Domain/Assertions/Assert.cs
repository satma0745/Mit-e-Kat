namespace Mitekat.Discovery.Domain.Assertions;

using System;

internal static class Assert
{
    public static T NotNull<T>(T value) =>
        value is null
            ? throw new AssertionException()
            : value;
    
    public static Guid NotEmpty(Guid value) =>
        value == Guid.Empty
            ? throw new AssertionException()
            : value;

    public static DateTime NotEmpty(DateTime value) =>
        value == default
            ? throw new AssertionException()
            : value;

    public static string NotNullOrWhiteSpace(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new AssertionException()
            : value;

    public static TimeSpan InclusiveBetween(TimeSpan value, TimeSpan lowerBound, TimeSpan upperBound) =>
        value < lowerBound || value > upperBound
            ? throw new AssertionException()
            : value;
}
