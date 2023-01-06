using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace DiscordBot;

public class Bot
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;
    
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public Bot(
        IConfiguration configuration, 
        IServiceProvider services)
    {
        _configuration = configuration;
        _services = services;
        
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info
        });
        
        _commands = new CommandService(new CommandServiceConfig
        {
            LogLevel = LogSeverity.Info,
            CaseSensitiveCommands = false
        });
        
        _client.Log += Log;
        _commands.Log += Log;
    }
    
    public async Task StartAsync()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _client.MessageReceived += HandleCommandAsync;

        var secretToken = _configuration["SecretToken"];
        await _client.LoginAsync(TokenType.Bot, secretToken);
        await _client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }
    
    private async Task HandleCommandAsync(SocketMessage arg)
    {
        // Bail out if it's a System Message.
        var msg = arg as SocketUserMessage;
        if (msg == null) return;

        // We don't want the bot to respond to itself or other bots.
        if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;
        
        // Create a number to track where the prefix ends and the command begins
        int pos = 0;
        // Replace the '!' with whatever character
        // you want to prefix your commands with.
        // Uncomment the second half if you also want
        // commands to be invoked by mentioning the bot instead.
        if (msg.HasCharPrefix('!', ref pos) /* || msg.HasMentionPrefix(_client.CurrentUser, ref pos) */)
        {
            // Create a Command Context.
            var context = new SocketCommandContext(_client, msg);
            
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully).
            var result = await _commands.ExecuteAsync(context, pos, _services);

            // Uncomment the following lines if you want the bot
            // to send a message if it failed.
            // This does not catch errors from commands with 'RunMode.Async',
            // subscribe a handler for '_commands.CommandExecuted' to see those.
            //if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            //    await msg.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
    
    private static Task Log(LogMessage message)
    {
        switch (message.Severity)
        {
            case LogSeverity.Critical:
            case LogSeverity.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case LogSeverity.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case LogSeverity.Info:
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case LogSeverity.Verbose:
            case LogSeverity.Debug:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                break;
        }
        Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}