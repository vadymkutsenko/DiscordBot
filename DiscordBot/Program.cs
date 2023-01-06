using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot;

internal static class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((host, services) =>
            {
                services.ConfigureServices(host);
            }).AddExternalConfiguration();


    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var bot = host.Services.GetRequiredService<Bot>();
        await bot.StartAsync();
    }
}



    