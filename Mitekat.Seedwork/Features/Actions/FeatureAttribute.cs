namespace Mitekat.Seedwork.Features.Actions;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class FeatureAttribute : Attribute
{
    public virtual string Group { get; }
    
    public virtual string Action { get; }

    public FeatureAttribute(string group, string action)
    {
        Group = group;
        Action = action;
    }
}
