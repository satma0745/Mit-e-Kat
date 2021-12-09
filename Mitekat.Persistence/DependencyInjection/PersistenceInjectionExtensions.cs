namespace Mitekat.Persistence.DependencyInjection;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Core.Abstractions.Repositories;
using Mitekat.Persistence.Context;
using Mitekat.Persistence.Repositories;

public static class PersistenceInjectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddMitekatContext()
            .AddRepositories();

    private static IServiceCollection AddMitekatContext(this IServiceCollection services) =>
        services.AddDbContext<MitekatContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("PostgreSQL");

            var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

            options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
        });

    // TODO: Refactor using reflection.
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IMeetupRepository, MeetupRepository>();
}
