using Application.Settings;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Application.Musics;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure.Loggings;
using Application.Bots;

namespace Infrastructure.Discord;
public class BotClient : IAsyncBotClient
{
    private DiscordSocketClient Client { get; set; }
    
    private IServiceProvider Provider { get; }
    
    private InteractionService InteractionService { get; }
    
    private BotSettings Settings { get; }
    
    private DiscordLogger DiscordLogger { get; }
    
    private bool IsVoiceChannelInit { get; set; } = false;

    public BotClient(BotSettings settings, IServiceCollection services)
    {
        this.Settings = settings;

        this.Client = GetInitializedClient();
        this.Provider = services.BuildServiceProvider();
        this.DiscordLogger = new DiscordLogger(new LocalFile.FileRepository());
        this.InteractionService = new InteractionService(Client.Rest);
    }

    public async Task StartAsync(ILogPrintable printable)
    {
        this.DiscordLogger.Register(this.Client, this.InteractionService, printable);
        this.DiscordLogger.WriteBotSystemLog("Application Name : " + Settings.BotName);
        this.DiscordLogger.WriteBotSystemLog("Version : " + Settings.Version.Major + "." + Settings.Version.Minor + "." + Settings.Version.Patch);
        this.DiscordLogger.WriteBotSystemLog("Language : " + Settings.BotLanguage);
        this.Client.InteractionCreated += ExecuteSlashCommand;
        this.Client.Ready += RegisterCommands;
        this.Client.Ready += VoiceChannelUpdate;
        await ConnectAsync();
    }

    public async Task StopAsync()
    {
        await DisconnectAsync();
        await Task.Delay(1000);
        this.Client.Dispose();
        this.Client = this.GetInitializedClient();
    }

    public async Task RestartAsync(ILogPrintable printable)
    {
        await StopAsync();
        await StartAsync(printable);
    }


    private async Task ConnectAsync()
    {
        await this.Client.LoginAsync(TokenType.Bot, this.Settings.DiscordToken);
        await this.Client.StartAsync();
    }

    private async Task DisconnectAsync()
    {
        await this.Client.StopAsync();
        await this.Client.LogoutAsync();
    }

    public async Task SetModulesAsync(IEnumerable<Assembly> assemblyOfModules)
    {
        foreach (var module in assemblyOfModules)
        {
            await this.InteractionService.AddModulesAsync(assembly: module, services: this.Provider);
        }
    }

    private async Task RegisterCommands()
    {
        await this.InteractionService.RegisterCommandsToGuildAsync(1019297738640330803);
        await this.InteractionService.RegisterCommandsGloballyAsync();
    }

    private async Task ExecuteSlashCommand(SocketInteraction interaction)
    {
        var socketInteractionContext = new SocketInteractionContext(this.Client, interaction);
        await this.InteractionService.ExecuteCommandAsync(socketInteractionContext, this.Provider);
    }

    private DiscordSocketClient GetInitializedClient()
    {
        var socketConfig = new DiscordSocketConfig()
        {
            LogLevel = LogSeverity.Info,
        };
        return new DiscordSocketClient(socketConfig);
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
