namespace Mitekat.Model.Extensions.DependencyInjection;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mitekat.Model.Context;

public static class ModelInjectionExtensions
{
    public static IServiceCollection AddMitekatContext(this IServiceCollection services) =>
        services.AddDbContext<MitekatContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("PostgreSQL");

            var modelsAssembly = Assembly.GetExecutingAssembly().FullName;

            options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(modelsAssembly));
        });
}
