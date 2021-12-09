﻿namespace Mitekat.Web.EntryPoint;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

internal static class Program
{
    private static void Main(string[] arguments) =>
        CreateHostBuilder(arguments).Build().Run();

    private static IHostBuilder CreateHostBuilder(string[] arguments) =>
        Host.CreateDefaultBuilder(arguments)
            .ConfigureAppConfiguration((host, configuration) =>
            {
                var baseConfig = Path.Combine("Properties", "appSettings.json");
                configuration.AddJsonFile(baseConfig, optional: true);

                var environmentName = host.HostingEnvironment.EnvironmentName;
                var environmentSpecificConfig = Path.Combine("Properties", $"appSettings.{environmentName}.json");
                configuration.AddJsonFile(environmentSpecificConfig, optional: true);
            })
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });
}
