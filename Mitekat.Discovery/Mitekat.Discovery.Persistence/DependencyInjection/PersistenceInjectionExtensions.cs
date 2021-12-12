namespace Mitekat.Discovery.Persistence.DependencyInjection;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Discovery.Core.Abstraction.Repositories;
using Mitekat.Discovery.Core.Seedwork.Configuration;
using Mitekat.Discovery.Persistence.Context;
using Mitekat.Discovery.Persistence.Repositories;

public static class PersistenceInjectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddMitekatContext()
            .AddRepositories();

    private static IServiceCollection AddMitekatContext(this IServiceCollection services) =>
        services
            .AddConfiguration<PersistenceConfiguration>()
            .AddDbContext<DiscoveryContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<PersistenceConfiguration>();
                var connectionString = configuration.ConnectionString;

                var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
            });

    // TODO: Refactor using reflection.
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IMeetupRepository, MeetupRepository>();
}
