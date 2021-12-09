namespace Mitekat.Web.Configuration.FeatureConvention;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

internal static class FeatureConventionConfigurationExtensions
{
    public static IServiceCollection ConfigureBindingConventions(this IServiceCollection services) =>
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressInferBindingSourcesForParameters = true;
        });

    public static IMvcBuilder ConfigureControllerConventions(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.ConfigureApplicationPartManager(applicationPart =>
        {
            var convention = new FeatureConventionProvider();
            applicationPart.FeatureProviders.Add(convention);
        });
}
