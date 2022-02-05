using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace plannerBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // This allows specifying environment on command line - ie dotnet run --environment "Staging"
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            CreateHostBuilder(args, config).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseSetting("detailedErrors", "true");
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>().UseUrls(config["Endpoint"]);
                    webBuilder.CaptureStartupErrors(true);

                    webBuilder.UseDefaultServiceProvider((context, options) => {
                        options.ValidateScopes = true;

                    });

                });
    }
}