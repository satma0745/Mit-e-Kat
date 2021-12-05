namespace Mitekat.Application.Conventions;

using System;

[AttributeUsage(AttributeTargets.Class)]
internal class FeatureAttribute : Attribute
{
    public string Group { get; }
    
    public string Action { get; }

    public FeatureAttribute(string group, string action)
    {
        Group = group;
        Action = action;
    }
}
