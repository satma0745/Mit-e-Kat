namespace Mitekat.Discovery.Domain.Seedwork;

using System;

internal class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }
}
