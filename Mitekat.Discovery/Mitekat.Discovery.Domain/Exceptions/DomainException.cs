namespace Mitekat.Discovery.Domain.Exceptions;

using System;

internal class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }
}
