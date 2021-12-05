namespace Mitekat.Application.Seedwork;

public class Response<TResource>
{
    public static Response<TResource> Success(TResource resource) =>
        new()
        {
            IsSuccess = true,
            Resource = resource,
            Error = default
        };

    public static Response<TResource> Failure(Error error) =>
        new()
        {
            IsSuccess = false,
            Resource = default,
            Error = error
        };

    public bool IsSuccess { get; private set; }
    
    public TResource Resource { get; private set; }
    
    public Error Error { get; private set; }
}

public record Error
{
    private Error()
    {
    }

    public sealed record NotFoundError : Error;

    public sealed record ConflictError : Error;
}
