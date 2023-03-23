using Discord.Interactions;
using Application.Interface;
using System.Reflection;
using Application.Musics;
using Discord.WebSocket;
using Discord;
using System.Runtime.CompilerServices;

namespace UI.Modules.MusicModule
{
    public class MusicModule : ModuleBase, IAssembleGetable
    {
        public MusicModule(IServiceProvider services) : base(services)
        {
            this.Musics = new Musics(services);
        }

        private Musics Musics { get; }

        [SlashCommand("musicplayer", "experimental musicplayer")]
        public async Task MusicPlayer()
        {
            try
            {
                await this.DeferAsync();
                await this.DeleteOriginalResponseAsync();
                var voiceChannel = ((SocketGuildUser)Context.User).VoiceChannel;
                if (voiceChannel is null) throw new Exception(Musics.Language["Application.Musics.Musics.InvalidVoiceChannelExecption"]);
                var componentBuilder = GetComponentBuilder();

                var builder = Musics.Queue(voiceChannel);

                var controllerMessage = await this.ReplyAsync(Musics.Language["UI.Modules.MusicModule.MusicModule.Queue.SentQueue"], components: componentBuilder.Build(), embed: builder.Build());

                var controller = Musics.GetController(voiceChannel);
                if (controller is not null) await controller.DeleteAsync();
                Musics.SetController(voiceChannel, controllerMessage);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        [SlashCommand("remove", "Remove the music from the queue.")]
        public async Task Remove(int index)
        {
            try
            {
                var msg = Musics.Remove(((SocketGuildUser)Context.User).VoiceChannel, index);
                await this.RespondAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        internal static ComponentBuilder GetComponentBuilder(bool isShuffle = false, string loopType = "normal", int volume = 0)
        {
            var componentBuilder = new ComponentBuilder();

            var page = new ActionRowBuilder();
            page = page
                .WithButton(new ButtonBuilder("previous", "previous-page"))
                .WithButton(new ButtonBuilder("now", "now-page"))
                .WithButton(new ButtonBuilder("next", "next-page"));

            var musicController = new ActionRowBuilder();
            musicController = musicController
                .WithButton(new ButtonBuilder("add", "add"))
                .WithButton(new ButtonBuilder("pause", "pause"))
                .WithButton(new ButtonBuilder("exit", "exit"))
                .WithButton(new ButtonBuilder("skip", "skip"));

            var queueRule = new ActionRowBuilder();
            queueRule = queueRule.WithButton(isShuffle ? new ButtonBuilder("shuffle On", "shuffle") : new ButtonBuilder("shuffle Off", "shuffle"));
            if(loopType == "normal")
            {
                queueRule = queueRule.WithButton(new ButtonBuilder("normal", "loop"));
            }
            else if(loopType == "queue")
            {
                queueRule = queueRule.WithButton(new ButtonBuilder("queue loop", "loop"));
            }
            else if(loopType == "oneSong")
            {
                queueRule = queueRule.WithButton(new ButtonBuilder("one song", "loop"));
            }
            else
            {
                throw new ArgumentException("loopType is invalid");
            }

            var volumeController = new ActionRowBuilder();
            volumeController = volumeController
                .WithButton(new ButtonBuilder("volume up", "volume-up"))
                .WithButton(new ButtonBuilder(volume.ToString(), "volume-default"))
                .WithButton(new ButtonBuilder("volume down", "volume-down"));

            componentBuilder = componentBuilder
                .AddRow(page)
                .AddRow(musicController)
                .AddRow(queueRule)
                .AddRow(volumeController);

            return componentBuilder;
        }

        public Assembly? GetAssembly()
        {
            return Assembly.GetAssembly(typeof(MusicModule));
        }
    }
}
