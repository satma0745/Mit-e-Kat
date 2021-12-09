namespace Mitekat.Core.Seedwork.Behaviors;

using System;

internal static class ReflectionExtensions
{
    public static bool IsSubclassOfGeneric(this Type target, Type @base)
    {
        var current = target.IsGenericType
            ? target.GetGenericTypeDefinition()
            : target;

        if (current == @base)
        {
            return false;
        }
        
        while (current is not null)
        {
            current = current.BaseType?.IsGenericType == true
                ? current.BaseType.GetGenericTypeDefinition()
                : current.BaseType;

            if (current == @base)
            {
                return true;
            }
        }
        return false;
    }
}
