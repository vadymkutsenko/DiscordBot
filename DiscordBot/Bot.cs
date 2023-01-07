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
            LogLevel = LogSeverity.Info,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
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
        if (arg is not SocketUserMessage message) return;
        
        int argPos = 0;

        if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot)
            return;

        var context = new SocketCommandContext(_client, message);
        
        var result = await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: _services);
        

        if (!result.IsSuccess)
        {
            if (result.Error == CommandError.UnknownCommand)
            {
                await message.Channel.SendMessageAsync("Трясця, я тебе не розумію");   
            }
            else
            {
                await message.Channel.SendMessageAsync("А йди ти до дідька");   
            }
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