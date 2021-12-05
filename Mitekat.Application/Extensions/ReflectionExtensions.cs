namespace Mitekat.Application.Extensions;

using System;
using System.Reflection;
using Mitekat.Application.Seedwork;

internal static class ReflectionExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute =>
        (TAttribute) type.GetCustomAttribute(typeof(TAttribute));

    public static bool IsActionClass(this Type type) =>
        type.IsClass &&
        !type.IsAbstract &&
        !type.ContainsGenericParameters &&
        type.IsNestedPublic &&
        type.IsSubclassOf(typeof(ActionBase));
}
