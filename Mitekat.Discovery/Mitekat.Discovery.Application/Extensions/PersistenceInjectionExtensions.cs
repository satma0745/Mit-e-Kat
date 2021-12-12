namespace Mitekat.Discovery.Application.Extensions;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Discovery.Application.Persistence.Context;
using Mitekat.Discovery.Application.Persistence.Repositories;
using Mitekat.Seedwork.Configuration;
using Mitekat.Seedwork.Persistence.Configuration;

public static class PersistenceInjectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddMitekatContext()
            .AddRepositories();

    private static IServiceCollection AddMitekatContext(this IServiceCollection services) =>
        services
            .AddConfiguration<IPersistenceConfiguration, PersistenceConfiguration>()
            .AddDbContext<DiscoveryContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IPersistenceConfiguration>();
                var connectionString = configuration.ConnectionString;

                var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
            });

    // TODO: Refactor using reflection.
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IMeetupRepository, MeetupRepository>();
}
