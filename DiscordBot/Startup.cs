using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext host)
    {
        services.AddSingleton<Bot>();

    }
    
    public static IHostBuilder AddExternalConfiguration(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration(cfg => cfg
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables());
    }
}