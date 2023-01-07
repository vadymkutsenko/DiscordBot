using Discord.Commands;

namespace DiscordBot.Actions.RepeatMessageAction;

public class RepeatMessageHandlerModule : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("Echoes a message.")]
    public Task SayAsync([Remainder] [Summary("The text to echo")] string echo) => ReplyAsync(echo);
}