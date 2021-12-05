namespace Mitekat.Application.Extensions;

using Microsoft.AspNetCore.Mvc.ApplicationParts;

internal static class ApplicationPartExtensions
{
    public static void UseFeatureProvider<TFeatureProvider>(this ApplicationPartManager applicationPart)
        where TFeatureProvider : IApplicationFeatureProvider, new() =>
        applicationPart.FeatureProviders.Add(new TFeatureProvider());
}
