namespace Mitekat.Auth.Core.Seedwork.Configuration;

using System;
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

    public static ConfigurationParameter<int> Integer(this ConfigurationParameter<string> parameter) =>
        !int.TryParse(parameter.Value, out var parsed)
            ? throw new ConfigurationValidationException($"Configuration error: {parameter.Name} must be an integer.")
            : parameter.Transform(_ => parsed);

    public static TimeSpan ToTimeAsMinutes(this ConfigurationParameter<int> parameter) =>
        TimeSpan.FromMinutes(parameter.Value);

    public static TimeSpan ToTimeAsDays(this ConfigurationParameter<int> parameter) =>
        TimeSpan.FromDays(parameter.Value);

    private static ConfigurationParameter<TResult> Transform<TSource, TResult>(
        this ConfigurationParameter<TSource> parameter,
        Func<TSource, TResult> transformer) =>
        new()
        {
            Name = parameter.Name,
            Value = transformer(parameter.Value)
        };
}
