namespace Mitekat.Seedwork.Web.Extensions;

using System;
using System.Reflection;

internal static class ReflectionExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute =>
        (TAttribute) type.GetCustomAttribute(typeof(TAttribute));
    
    public static bool HasAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute =>
        type.GetAttribute<TAttribute>() is not null;
}
