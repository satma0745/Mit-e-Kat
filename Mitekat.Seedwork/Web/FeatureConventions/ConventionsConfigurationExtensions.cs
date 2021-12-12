namespace Mitekat.Seedwork.Web.FeatureConventions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

public static class ConventionsConfigurationExtensions
{
    public static IServiceCollection DisableBindingSourcesInference(this IServiceCollection services) =>
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressInferBindingSourcesForParameters = true;
        });

    public static ApplicationPartManager AddConvention<TConvention>(this ApplicationPartManager applicationPart)
        where TConvention : IApplicationFeatureProvider, new()
    {
        var conventionFeatureProvider = new TConvention();
        applicationPart.FeatureProviders.Add(conventionFeatureProvider);

        return applicationPart;
    }
}
