using Discord.Commands;

namespace DiscordBot.Actions.HelpAction;

public class HelpHandlerModule : ModuleBase<SocketCommandContext>
{
    [Command("help")]
    public async Task Help()
    {
        await Context.Channel.SendMessageAsync("For adding song to public playlist pls execute following command:\n " +
                                               "!playlist enrich title:{songTitle} artist:{artistName}\n" +
                                               "For example:\n" +
                                               "!playlist enrich title:\"Cryptonite\" artist:\"Three Days Grace\"");
    }
}