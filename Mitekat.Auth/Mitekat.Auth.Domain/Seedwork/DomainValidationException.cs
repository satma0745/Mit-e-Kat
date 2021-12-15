namespace Mitekat.Auth.Domain.Seedwork;

internal class DomainValidationException : DomainException
{
    private DomainValidationException(string message)
        : base(message)
    {
    }

    public static DomainValidationException Required(string parameterName) =>
        new($"{parameterName} is required.");

    public static DomainValidationException StringMinLength(string parameterName, int minLength) =>
        new($"{parameterName} should be at least {minLength} characters long.");

    public static DomainValidationException StringMaxLength(string parameterName, int maxLength) =>
        new($"{parameterName} should not exceed {maxLength} characters.");
}
