namespace Mitekat.Auth.Persistence.DependencyInjection;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Auth.Core.Abstractions.Repositories;
using Mitekat.Auth.Persistence.Context;
using Mitekat.Auth.Persistence.Repositories;

public static class PersistenceInjectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddMitekatContext()
            .AddRepositories();

    private static IServiceCollection AddMitekatContext(this IServiceCollection services) =>
        services.AddDbContext<AuthContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var server = configuration["Persistence:Server"];
            var port = configuration["Persistence:Port"];
            var db = configuration["Persistence:Database"];
            var username = configuration["Persistence:Username"];
            var password = configuration["Persistence:Password"];
            var connectionString = $"Server={server};Port={port};Database={db};User Id={username};Password={password};";

            var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

            options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
        });

    // TODO: Refactor using reflection.
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IUserRepository, UserRepository>();
}
