namespace Mitekat.Discovery.Core.Seedwork.Configuration;

using Microsoft.Extensions.Configuration;

public static class ConfigurationParameterExtensions
{
    public static ConfigurationParameter<string> GetParameter(
        this IConfiguration configuration,
        string path,
        string name) =>
        new()
        {
            Name = name,
            Value = configuration[path]
        };

    public static ConfigurationParameter<string> Required(this ConfigurationParameter<string> parameter) =>
        string.IsNullOrWhiteSpace(parameter.Value)
            ? throw new ConfigurationValidationException($"Configuration error: {parameter.Name} is required.")
            : parameter;
}
