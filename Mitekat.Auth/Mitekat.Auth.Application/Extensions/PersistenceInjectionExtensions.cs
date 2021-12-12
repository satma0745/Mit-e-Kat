namespace Mitekat.Auth.Application.Extensions;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Auth.Application.Persistence.Context;
using Mitekat.Auth.Application.Persistence.Repositories;
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
            .AddDbContext<AuthContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IPersistenceConfiguration>();
                var connectionString = configuration.ConnectionString;

                var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
            });

    // TODO: Refactor using reflection.
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IUserRepository, UserRepository>();
}
