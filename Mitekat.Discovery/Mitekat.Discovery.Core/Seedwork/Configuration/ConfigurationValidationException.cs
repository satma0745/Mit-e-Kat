namespace Mitekat.Discovery.Core.Seedwork.Configuration;

using System;

internal class ConfigurationValidationException : Exception
{
    public ConfigurationValidationException(string message)
        : base(message)
    {
    }
}
