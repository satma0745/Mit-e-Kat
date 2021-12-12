namespace Mitekat.Seedwork.Configuration;

using System;

public class ConfigurationValidationException : Exception
{
    public ConfigurationValidationException(string message)
        : base(message)
    {
    }
}
