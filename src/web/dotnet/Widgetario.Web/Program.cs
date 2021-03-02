using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;

namespace Widgetario.Web
{
    public class Program
    {
        private static readonly Gauge _InfoGauge = 
            Metrics.CreateGauge("app_info", "Application info", "dotnet_version", "assembly_name", "app_version");

        public static void Main(string[] args)
        {
            _InfoGauge.Labels("3.1.7", "Widgetario.Web", "1.1.0").Set(1);
            
            Log.Logger = new LoggerConfiguration()            
                                .Enrich.FromLogContext()
                                .MinimumLevel.Information()
                                .WriteTo.File("/logs/app.log", shared: true, flushToDiskInterval: TimeSpan.FromSeconds(5))
                                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json")
                          .AddEnvironmentVariables()
                          .AddJsonFile("config/logging.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("secrets/api.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
