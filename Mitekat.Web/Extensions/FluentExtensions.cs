namespace Mitekat.Web.Extensions;

using System;

internal static class FluentExtensions
{
    public static T If<T>(this T pivot, Func<bool> condition, Action action)
    {
        if (condition())
        {
            action();
        }

        return pivot;
    }
}
