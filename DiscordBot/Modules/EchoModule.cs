using Discord.Commands;

namespace DiscordBot.Modules;

public class EchoModule : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("Echoes a message.")]
    public Task SayAsync([Remainder] [Summary("The text to echo")] string echo) => ReplyAsync(echo);
}