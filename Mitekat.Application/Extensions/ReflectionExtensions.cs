namespace Mitekat.Application.Extensions;

using System;
using System.Reflection;
using Mitekat.Application.Conventions;
using Mitekat.Application.Seedwork;

internal static class ReflectionExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute =>
        (TAttribute) type.GetCustomAttribute(typeof(TAttribute));
    
    public static bool HasAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute =>
        type.GetAttribute<TAttribute>() is not null;

    public static bool IsActionClass(this Type type) =>
        type.IsClass &&
        type.IsPublic &&
        !type.IsAbstract &&
        !type.ContainsGenericParameters &&
        type.IsSubclassOf(typeof(ActionBase)) &&
        type.HasAttribute<FeatureAttribute>();
}
