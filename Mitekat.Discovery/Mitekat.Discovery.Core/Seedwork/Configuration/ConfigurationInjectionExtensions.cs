namespace Mitekat.Discovery.Core.Seedwork.Configuration;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigurationInjectionExtensions
{
    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services)
        where TConfiguration : class
    {
        var constructorParameters = new[] {typeof(IConfiguration)};
        var constructor = typeof(TConfiguration).GetConstructor(constructorParameters);

        if (constructor is null)
        {
            throw new Exception($"Cannot instantiate configuration of type {typeof(TConfiguration)}");
        }

        return services.AddScoped(serviceProvider =>
        {
            var applicationConfiguration = serviceProvider.GetRequiredService<IConfiguration>();
            var constructorArguments = new object[] {applicationConfiguration};
            
            var configuration = constructor.Invoke(constructorArguments);
            return configuration as TConfiguration;
        });
    }
}
