namespace Mitekat.Auth.Persistence.DependencyInjection;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Auth.Core.Abstractions.Repositories;
using Mitekat.Auth.Core.Seedwork.Configuration;
using Mitekat.Auth.Persistence.Context;
using Mitekat.Auth.Persistence.Repositories;

public static class PersistenceInjectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddMitekatContext()
            .AddRepositories();

    private static IServiceCollection AddMitekatContext(this IServiceCollection services) =>
        services
            .AddConfiguration<PersistenceConfiguration>()
            .AddDbContext<AuthContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<PersistenceConfiguration>();
                var connectionString = configuration.ConnectionString;

                var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
            });

    // TODO: Refactor using reflection.
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IUserRepository, UserRepository>();
}
