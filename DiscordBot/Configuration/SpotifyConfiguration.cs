using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Configuration;

public static class SpotifyConfiguration
{
    public const string ClientName = "Spotify";
    
    public static void AddSpotifyHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient(ClientName, (_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["Spotify:BaseUrl"]!);
            httpClient.Timeout = TimeSpan.FromMilliseconds(int.Parse(configuration["Spotify:Timeout"]!));

            var token = configuration["Spotify:AuthorizationToken"]!;
            var authorizationHeader = new AuthenticationHeaderValue(
                scheme: "Bearer",
                parameter: token);

            httpClient.DefaultRequestHeaders.Authorization = authorizationHeader;
        });
    }
}