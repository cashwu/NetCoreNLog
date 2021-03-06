using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace testNetCoreNlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Setup()
                      .SetupExtensions(a => a.RegisterLayoutRenderer<UserNameLayoutRenderer>("user-name"));

            // var config = new ConfigurationBuilder()
            //              .AddJsonFile("AppSettings.json")
            //              .Build();
            //
            // NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = config;

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");

                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddConsole();
                    
                    ConfigSettingLayoutRenderer.DefaultConfiguration = context.Configuration;
                })
                .UseNLog();
    }
}