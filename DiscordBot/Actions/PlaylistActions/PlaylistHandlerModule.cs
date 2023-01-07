using Discord.Commands;
using DiscordBot.Actions.PlaylistActions.Inputs;
using DiscordBot.Services;

namespace DiscordBot.Actions.PlaylistActions;

[Group("playlist")]
public class PlaylistHandlerModule : ModuleBase<SocketCommandContext>
{
    private readonly SpotifyTracksSearchService _spotifyTracksSearchService;
    private readonly SpotifyPlaylistService _spotifyPlaylistService;

    public PlaylistHandlerModule(
        SpotifyTracksSearchService spotifyTracksSearchService, 
        SpotifyPlaylistService spotifyPlaylistService)
    {
        _spotifyTracksSearchService = spotifyTracksSearchService;
        _spotifyPlaylistService = spotifyPlaylistService;
    }

    [Command("enrich")]
    public async Task Enrich(EnrichPlaylistInput input)
    {
        var trackUri = await _spotifyTracksSearchService.Search(input.Title, input.Artist);
        
        if (trackUri is null)
        {
            await ReplyAsync($"trackId was not found by received input: title:{input.Title}; artist:{input.Artist}");
            return;
        }

        var isSuccessfullyAdded = await _spotifyPlaylistService.AddTrack(trackUri.Uri);
        if (isSuccessfullyAdded)
        {
            await ReplyAsync($"track has been successfully added; trackUri: {trackUri}");
        }
        else
        {
            await ReplyAsync("track has not been added");
        }
    }
}