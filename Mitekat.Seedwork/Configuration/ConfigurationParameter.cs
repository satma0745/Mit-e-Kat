namespace Mitekat.Seedwork.Configuration;

public class ConfigurationParameter<T>
{
    public virtual string Name { get; init; }
    public virtual T Value { get; init; }

    public static implicit operator T(ConfigurationParameter<T> parameter) =>
        parameter.Value;
}
