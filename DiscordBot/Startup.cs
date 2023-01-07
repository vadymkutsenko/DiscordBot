using DiscordBot.Configuration;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext host)
    {
        services.AddHttpClient();
        services.AddSpotifyHttpClient(host.Configuration);
        services.AddSingleton<Bot>();
        services.AddSingleton<SpotifyPlaylistService>();
        services.AddSingleton<SpotifyTracksSearchService>();
    }
    
    public static IHostBuilder AddExternalConfiguration(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration(cfg => cfg
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables());
    }
}