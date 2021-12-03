namespace Mitekat.Application;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

internal static class Program
{
    private static void Main(string[] arguments) =>
        CreateHostBuilder(arguments).Build().Run();

    private static IHostBuilder CreateHostBuilder(string[] arguments) =>
        Host.CreateDefaultBuilder(arguments)
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
}
