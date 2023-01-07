using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Web;
using DiscordBot.Configuration;

namespace DiscordBot.Services;

public class SpotifyTracksSearchService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SpotifyTracksSearchService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<SearchResult?> Search(string title, string artist)
    {
        var httpClient = _httpClientFactory.CreateClient(SpotifyConfiguration.ClientName);

        var encodedTitle = HttpUtility.UrlEncode(title);
        var encodedArtist = HttpUtility.UrlEncode(artist);
        var searchUrl = $"v1/search?q=track:{encodedTitle}%20artist:{encodedArtist}&type=track&limit=1&offset=0";
        
        var httpResponse = await httpClient.GetAsync(searchUrl);
        var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();

        var foundItem = response?.Tracks?.Items.FirstOrDefault();
        if (foundItem is null) return null;

        return new SearchResult
        {
            Uri = foundItem.Uri
        };
    }
    
    public class SearchResult
    {
        public string Uri { get; set; }
    }


    private class ApiResponse
    {
        [JsonPropertyName("tracks")] 
        public TracksInfo? Tracks { get; set; }
        
    public record Album(
        [property: JsonPropertyName("album_type")] string AlbumType,
        [property: JsonPropertyName("artists")] IReadOnlyList<Artist> Artists,
        [property: JsonPropertyName("available_markets")] IReadOnlyList<string> AvailableMarkets,
        [property: JsonPropertyName("external_urls")] ExternalUrls ExternalUrls,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("images")] IReadOnlyList<Image> Images,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("release_date")] string ReleaseDate,
        [property: JsonPropertyName("release_date_precision")] string ReleaseDatePrecision,
        [property: JsonPropertyName("total_tracks")] int TotalTracks,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("uri")] string Uri
    );

    public record Artist(
        [property: JsonPropertyName("external_urls")] ExternalUrls ExternalUrls,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("uri")] string Uri
    );

    public record ExternalIds(
        [property: JsonPropertyName("isrc")] string Isrc
    );

    public record ExternalUrls(
        [property: JsonPropertyName("spotify")] string Spotify
    );

    public record Image(
        [property: JsonPropertyName("height")] int Height,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("width")] int Width
    );

    public record Item(
        [property: JsonPropertyName("album")] Album Album,
        [property: JsonPropertyName("artists")] IReadOnlyList<Artist> Artists,
        [property: JsonPropertyName("available_markets")] IReadOnlyList<string> AvailableMarkets,
        [property: JsonPropertyName("disc_number")] int DiscNumber,
        [property: JsonPropertyName("duration_ms")] int DurationMs,
        [property: JsonPropertyName("explicit")] bool Explicit,
        [property: JsonPropertyName("external_ids")] ExternalIds ExternalIds,
        [property: JsonPropertyName("external_urls")] ExternalUrls ExternalUrls,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("is_local")] bool IsLocal,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("popularity")] int Popularity,
        [property: JsonPropertyName("preview_url")] string PreviewUrl,
        [property: JsonPropertyName("track_number")] int TrackNumber,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("uri")] string Uri
    );

    public record TracksInfo(
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("items")] IReadOnlyList<Item> Items,
        [property: JsonPropertyName("limit")] int Limit,
        [property: JsonPropertyName("next")] string Next,
        [property: JsonPropertyName("offset")] int Offset,
        [property: JsonPropertyName("previous")] object Previous,
        [property: JsonPropertyName("total")] int Total
    );
    }
}