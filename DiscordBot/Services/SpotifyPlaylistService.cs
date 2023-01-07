using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Web;
using DiscordBot.Configuration;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Services;

public class SpotifyPlaylistService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _playlistId;

    public SpotifyPlaylistService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _playlistId = configuration["Spotify:PlaylistId"]!;
    }

    public async Task<bool> AddTrack(string uri)
    {
        var httpClient = _httpClientFactory.CreateClient(SpotifyConfiguration.ClientName);

        var encodedSpotifyUrl = HttpUtility.UrlEncode(uri);
        var httpResponse = await httpClient.PostAsync($"v1/playlists/{_playlistId}/tracks?position=0&uris={encodedSpotifyUrl}", new StringContent("application/json"));
        var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();
        return !string.IsNullOrWhiteSpace(response?.SnapshotId);
    }
    
    private record ApiResponse(
        [property: JsonPropertyName("snapshot_id")] string SnapshotId
    );
}