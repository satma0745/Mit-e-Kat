namespace Mitekat.Discovery.Application.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Mitekat.Seedwork.Web.FeatureConventions;

internal static class ApplicationPartsConfigurationExtensions
{
    public static IMvcBuilder ConfigureApplicationParts(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.ConfigureApplicationPartManager(applicationPart =>
        {
            applicationPart.AddConvention<FeatureConvention>();
        });
}
