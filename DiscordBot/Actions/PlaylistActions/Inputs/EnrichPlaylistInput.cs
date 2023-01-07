using Discord.Commands;

namespace DiscordBot.Actions.PlaylistActions.Inputs;

[NamedArgumentType]
public class EnrichPlaylistInput
{
    public string Title { get; set; }
    public string Artist { get; set; }
}