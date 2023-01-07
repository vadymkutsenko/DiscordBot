using Discord.Commands;

namespace DiscordBot.Modules;

[Group("calculate")]
public class CalculateModule : ModuleBase<SocketCommandContext>
{
    [Command("square")]
    [Summary("Squares a number.")]
    public async Task SquareAsync([Summary("The number to square.")] int num)
    {
        await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
    }
}