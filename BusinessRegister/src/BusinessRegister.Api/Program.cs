using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace BusinessRegister.Api
{
    /// <summary>
    /// Entry point of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of the whole program
        /// </summary>
        /// <param name="args">Arguments to run the program</param>
        public static void Main(string[] args)
        {
            try
            {
                var pathToContentRoot = AppContext.BaseDirectory;

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(pathToContentRoot)
                    .AddJsonFile("appsettings.json")
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .CreateLogger();

                BuildWebHost(args, configuration, pathToContentRoot).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Build the webhost for IIS
        /// </summary>
        /// <param name="args">Arguments for launch</param>
        /// <param name="conf">Inject configuration into startup</param>
        /// <param name="contentRoot">Where contnet is located</param>
        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot conf, string contentRoot) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(conf)
                .Build();
    }
}
