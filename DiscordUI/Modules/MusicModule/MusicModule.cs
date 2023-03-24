using Discord.Interactions;
using Application.Interface;
using System.Reflection;
using Domain.Musics.Queue;
using Application.Musics;
using Discord.WebSocket;
using Discord;
using System.Runtime.CompilerServices;

namespace DiscordUI.Modules.MusicModule
{
    public class MusicModule : ModuleBase
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
                var componentBuilder = GetComponentBuilder(new PlayingOption(false, new NormalMusicQueueState(), 0));

                var builder = Musics.Queue(voiceChannel);

                var controllerMessage = await this.ReplyAsync(Musics.Language["DiscordUI.Modules.MusicModule.MusicModule.Queue.SentQueue"], components: componentBuilder.Build(), embed: builder.Build());

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

        internal static ComponentBuilder GetComponentBuilder(PlayingOption playingOption)
        {
            var componentBuilder = new ComponentBuilder();

            var page = new ActionRowBuilder();
            page = page
                .WithButton(new ButtonBuilder("previous", "previous-page"))
                .WithButton(new ButtonBuilder("now", "now-page"))
                .WithButton(new ButtonBuilder("next", "next-page"));

            var musicController = new ActionRowBuilder();
            musicController = musicController
                .WithButton(new ButtonBuilder(label: "add musics", customId: "add", emote: Emoji.Parse(":musical_note:")))
                .WithButton(new ButtonBuilder(customId: "pause", emote: Emoji.Parse(":play_pause:")))
                .WithButton(new ButtonBuilder(customId: "exit", emote: Emoji.Parse(":stop_button:")))
                .WithButton(new ButtonBuilder(customId: "skip", emote: Emoji.Parse(":track_next:")));

            var queueRule = new ActionRowBuilder();
            queueRule = queueRule.WithButton(playingOption.IsShuffle ? new ButtonBuilder(customId: "shuffle", style: ButtonStyle.Success, emote: Emoji.Parse(":twisted_rightwards_arrows:"))
                : new ButtonBuilder(customId: "shuffle", style: ButtonStyle.Danger, emote: Emoji.Parse(":twisted_rightwards_arrows:")));
            if (playingOption.QueueState is NormalMusicQueueState)
            {
                queueRule = queueRule.WithButton(new ButtonBuilder(customId: "loop", emote: Emoji.Parse(":arrow_right:")));
            }
            else if (playingOption.QueueState is QueueLoopMusicQueueState)
            {
                queueRule = queueRule.WithButton(new ButtonBuilder(customId: "loop", emote: Emoji.Parse(":repeat:")));
            }
            else if (playingOption.QueueState is OneSongLoopMusicQueueState)
            {
                queueRule = queueRule.WithButton(new ButtonBuilder(customId: "loop", emote: Emoji.Parse(":repeat_one:")));
            }
            else
            {
                throw new ArgumentException("loopType is invalid");
            }

            var volumeController = new ActionRowBuilder();
            volumeController = volumeController
                .WithButton(new ButtonBuilder(customId: "volume-down", style: ButtonStyle.Success, emote: Emoji.Parse(":speaker:")))
                .WithButton(new ButtonBuilder(label: playingOption.Volume.ToString(), customId: "volume-default", style: ButtonStyle.Success, emote: Emoji.Parse(":sound:")))
                .WithButton(new ButtonBuilder(customId: "volume-up", style: ButtonStyle.Success, emote: Emoji.Parse(":loud_sound:")));


            componentBuilder = componentBuilder
                .AddRow(page)
                .AddRow(musicController)
                .AddRow(queueRule)
                .AddRow(volumeController);
            return componentBuilder;
        }
    }
}
