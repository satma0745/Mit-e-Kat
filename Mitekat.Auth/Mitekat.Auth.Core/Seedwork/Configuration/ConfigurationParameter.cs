namespace Mitekat.Auth.Core.Seedwork.Configuration;

public class ConfigurationParameter<T>
{
    public string Name { get; init; }
    public T Value { get; init; }

    public static implicit operator T(ConfigurationParameter<T> parameter) =>
        parameter.Value;
}
