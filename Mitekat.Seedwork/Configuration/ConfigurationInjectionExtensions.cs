namespace Mitekat.Seedwork.Configuration;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigurationInjectionExtensions
{
    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services)
        where TConfiguration : class =>
        services.AddConfiguration<TConfiguration, TConfiguration>();
    
    public static IServiceCollection AddConfiguration<TConfiguration, TImplementation>(this IServiceCollection services)
        where TConfiguration : class
        where TImplementation : class, TConfiguration
    {
        var constructorParameters = new[] {typeof(IConfiguration)};
        var constructor = typeof(TImplementation).GetConstructor(constructorParameters);

        if (constructor is null)
        {
            throw new Exception($"Cannot instantiate configuration of type {typeof(TImplementation)}");
        }

        return services.AddScoped<TConfiguration>(serviceProvider =>
        {
            var applicationConfiguration = serviceProvider.GetRequiredService<IConfiguration>();
            var constructorArguments = new object[] {applicationConfiguration};
            
            var configuration = constructor.Invoke(constructorArguments);
            return configuration as TImplementation;
        });
    }
}
