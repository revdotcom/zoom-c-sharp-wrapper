using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using zoombot.Services;

namespace zoombot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Length > 0)
            {
                BotLauncher.BotExe = args[0];
            }

            ((IHostApplicationLifetime)host.Services.GetService(typeof(IHostApplicationLifetime))).ApplicationStopped.Register(() => {
            if (BotLauncher.BootupBot is { })
            {
                    BotLauncher.BootupBot.Process.Kill();
            }
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://0.0.0.0:5000");
                });
    }
}
