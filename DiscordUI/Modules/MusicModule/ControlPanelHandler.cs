using Application.Musics;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace DiscordUI.Modules.MusicModule
{
    public class ControlPanelHandler : ModuleBase
    {
        public ControlPanelHandler(IServiceProvider services) : base(services)
        {
            this.Musics = new Musics(services);
        }

        private Musics Musics { get; }

        [ComponentInteraction("add")]
        public async Task Add()
        {
            var component = (SocketMessageComponent)this.Context.Interaction;
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;

            await Context.Interaction.RespondWithModalAsync<MusicUrlModal>("music-url-modal");
        }

        [ModalInteraction("music-url-modal")]
        public async Task ModalResponse(MusicUrlModal modal)
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            if (modal.Url is null) return;
            await this.Musics.Play(voiceChannel, modal.Url, UpdateQueueAsync);
            await UpdateQueueAsync(voiceChannel);
            await this.DeferAsync();
        }

        [ComponentInteraction("exit")]
        public async Task Exit()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            Musics.Exit(voiceChannel);
            var controller = this.Musics.GetController(voiceChannel);
            if(controller is not null) await controller.DeleteAsync();

        }

        [ComponentInteraction("previous-page")]
        public async Task Previous()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            Musics.AddCurrentQueuePage(voiceChannel, -1);
            await UpdateQueueAsync(voiceChannel);
            await this.DeferAsync();
        }

        [ComponentInteraction("now-page")]
        public async Task Now()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            Musics.SetCurrentQueuePage(voiceChannel, 1);
            await UpdateQueueAsync(voiceChannel);
            await this.DeferAsync();
        }


        [ComponentInteraction("next-page")]
        public async Task Next()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            Musics.AddCurrentQueuePage(voiceChannel, 1);
            await UpdateQueueAsync(voiceChannel);
            await this.DeferAsync();
        }

        [ComponentInteraction("skip")]
        public async Task Skip()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            this.Musics.Skip(voiceChannel);
            await UpdateQueueAsync(voiceChannel);
            await this.DeferAsync();
        }

        [ComponentInteraction("pause")]
        public async Task Pause()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            this.Musics.Pause(voiceChannel);
            await this.DeferAsync();
        }

        [ComponentInteraction("loop")]
        public async Task Loop()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            var playingOption = this.Musics.Loop(voiceChannel);
            
            await this.UpdateStateAsync(voiceChannel, playingOption);
            await this.DeferAsync();
        }

        [ComponentInteraction("shuffle")]
        public async Task Shuffle()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            var playingOption = this.Musics.Shuffle(voiceChannel);
            await this.UpdateStateAsync(voiceChannel, playingOption);
            await this.UpdateQueueAsync(voiceChannel);
            await this.DeferAsync();
        }

        [ComponentInteraction("volume-up")]
        public async Task VolumeUp()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            var option = this.Musics.Volume(voiceChannel, 20);
            await this.UpdateStateAsync(voiceChannel, option);
            await this.DeferAsync();
        }

        [ComponentInteraction("volume-default")]
        public async Task VolumeDefault()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            var option = this.Musics.Volume(voiceChannel, 0);
            await this.UpdateStateAsync(voiceChannel, option);
            await this.DeferAsync();
        }

        [ComponentInteraction("volume-down")]
        public async Task VolumeDown()
        {
            var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
            var option = this.Musics.Volume(voiceChannel, -20);
            await this.UpdateStateAsync(voiceChannel, option);
            await this.DeferAsync();
        }

        private async Task UpdateQueueAsync(IVoiceChannel voiceChannel)
        {
            await Task.Delay(500);
            var controller = this.Musics.GetController(voiceChannel);
            if(controller is not null) await controller.ModifyAsync(x => { x.Embed = this.Musics.Queue(voiceChannel).Build(); });
        }

        private async Task UpdateStateAsync(IVoiceChannel voiceChannel, PlayingOption option)
        {
            await Task.Delay(500);
            var controller = this.Musics.GetController(voiceChannel);
            if (controller is not null) await controller.ModifyAsync(x => { x.Components = MusicModule.GetComponentBuilder(option).Build(); });
        }

        public class MusicUrlModal : IModal
        {
            public string Title => "Add music";
            [InputLabel("Please input URL")]
            [ModalTextInput("url")]
            public string? Url { get; set; }
        }

    }
}
