using Application.Interface;
using Application.Settings;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Domain.Factory;
using Domain.Interface;
using Domain.Musics;
using Domain.YouTube;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Infrastructure.Discord;
public class BotClient
{
    private DiscordSocketClient Client { get; }
    IServiceProvider Provider { get; }
    InteractionService InteractionService { get; }
    private BotSettings Settings { get; }
    private Connecter DiscordConnecter { get; }
    private IDiscordLogger DiscordLogger { get; }
    private IConsoleReader ConsoleReader { get; }
    private bool IsVoiceChannelInit { get; set; } = false;

    public BotClient(BotSettings settings)
    {
        this.DiscordLogger = Factory.GetService<IDiscordLogger>();
        this.ConsoleReader = Factory.GetService<IConsoleReader>();
        this.DiscordConnecter = new Connecter();
        this.Settings = settings;


        Api.GetInstance().Setkey(this.Settings.YouTubeApiKey);

        var socketConfig = new DiscordSocketConfig()
        {
            LogLevel = LogSeverity.Info,
        };
        this.Client = new DiscordSocketClient(socketConfig);
        this.Provider = new ServiceCollection().BuildServiceProvider();
        this.InteractionService = new InteractionService(Client.Rest);
    }

    public async Task StartAsync()
    {
        this.DiscordLogger.WriteBotSystemLog("Application Name : " + Settings.BotName);
        this.DiscordLogger.WriteBotSystemLog("Version : " + Settings.Version.Major + "." + Settings.Version.Minor + "." + Settings.Version.Patch);
        this.DiscordLogger.WriteBotSystemLog("Language : " + Settings.BotLanguage);
        this.Client.InteractionCreated += ExecuteSlashCommand;
        this.Client.Ready += RegisterCommands;
        this.Client.Ready += VoiceChannelUpdate;
        this.DiscordLogger.Register(this.Client, this.InteractionService);
        await DiscordConnecter.ConnectAsync(this.Client, Settings.DiscordToken);
        this.ConsoleReader.ActiveConsole();
        await DiscordConnecter.DisconnectAsync(this.Client);
        await Task.Delay(1000);
        this.Client.Dispose();
    }

    public async Task SetModulesAsync(IReadOnlyCollection<Assembly> assemblyOfModules)
    {
        foreach (var module in assemblyOfModules)
        {
            await this.InteractionService.AddModulesAsync(assembly: module, services: this.Provider);
        }
    }

    private async Task RegisterCommands()
    {
        await this.InteractionService.RegisterCommandsGloballyAsync();
    }

    private async Task ExecuteSlashCommand(SocketInteraction interaction)
    {
        var socketInteractionContext = new SocketInteractionContext(this.Client, interaction);
        await this.InteractionService.ExecuteCommandAsync(socketInteractionContext, this.Provider);
    }

    private Task VoiceChannelUpdate()
    {
        if (this.IsVoiceChannelInit)
        {
            this.DiscordLogger.WriteBotSystemLog("start voice channel update");
            var musicPlayers = MusicPlayerProvider.GetAllMusicPlayers();
            foreach (var musicPlayer in musicPlayers)
            {
                var voiceChannel = (SocketVoiceChannel)this.Client.GetChannel(musicPlayer.VoiceChannelId);
                _ = Task.Run(async () =>
                {
                    await musicPlayer.UpdateAsync(voiceChannel);
                    this.DiscordLogger.WriteBotSystemLog("updated voice channel  guild : " + musicPlayer.GuildName + "  channel : " + musicPlayer.ChannelName);
                });
            }
        }
        this.IsVoiceChannelInit = true;
        return Task.CompletedTask;
    }
}
