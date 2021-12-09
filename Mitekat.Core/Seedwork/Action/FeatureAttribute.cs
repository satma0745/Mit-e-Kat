﻿namespace Mitekat.Core.Seedwork.Action;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class FeatureAttribute : Attribute
{
    public string Group { get; }
    
    public string Action { get; }

    public FeatureAttribute(string group, string action)
    {
        Group = group;
        Action = action;
    }
}