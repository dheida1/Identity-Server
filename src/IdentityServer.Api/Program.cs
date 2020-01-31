using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Security.Authentication;

namespace IdentityServer.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.Title = "IdentityServer4.EntityFramework";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication",
                                LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                                theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Starting host...");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.AllowSynchronousIO = true;
                        options.ConfigureHttpsDefaults(httpsOptions =>
                            {
                                httpsOptions.ServerCertificate = X509.LocalMachine.My.Thumbprint.Find("3BC996BE4AC586047AB08D01A9A6AB6276CF5837", false).Single();
                                httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                                httpsOptions.SslProtocols = SslProtocols.Tls12;
                            });
                    });
                });
    }
}
