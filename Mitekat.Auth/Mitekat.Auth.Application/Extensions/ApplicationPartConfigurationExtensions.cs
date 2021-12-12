namespace Mitekat.Auth.Application.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Seedwork.Web.FeatureConventions;

internal static class ApplicationPartConfigurationExtensions
{
    public static IMvcBuilder ConfigureApplicationParts(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.ConfigureApplicationPartManager(applicationPart =>
        {
            applicationPart.AddConvention<FeatureConvention>();
        });
}
