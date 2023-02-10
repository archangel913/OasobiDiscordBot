using Discord.Interactions;
using Application.Interface;
using System.Reflection;
using Application.Musics;
using Discord.WebSocket;

namespace UI.Modules.MusicModule
{
    public class MusicModule : InteractionModuleBase, IAssembleGetable
    {
        [SlashCommand("play", "Play the music/play list.")]
        public async Task Play(string url)
        {
            try
            {
                await this.RespondAsync(Musics.Language["UI.Modules.MusicModule.MusicModule.Play.Wait"]);
                string msg = await Musics.Play(((SocketGuildUser)Context.User).VoiceChannel, url);
                await this.ReplyAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }


        [SlashCommand("exit", "Exit from the voice channel.")]
        public async Task Exit()
        {
            try
            {
                await this.RespondAsync(Musics.Language["UI.Modules.MusicModule.MusicModule.Exit.Wait"]);
                string msg = Musics.Exit(((SocketGuildUser)Context.User).VoiceChannel);
                await this.ReplyAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        [SlashCommand("queue", "Send the queue.")]
        public async Task Queue(int page = 1)
        {
            try
            {
                var builder = Musics.Queue(page, ((SocketGuildUser)Context.User).VoiceChannel);
                await this.RespondAsync(Musics.Language["UI.Modules.MusicModule.MusicModule.Queue.SentQueue"], embed: builder.Build());
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        [SlashCommand("pause", "Toggle pause.")]
        public async Task Pause()
        {
            try
            {
                var msg = Musics.Pause(((SocketGuildUser)Context.User).VoiceChannel);
                await this.RespondAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        [SlashCommand("skip", "Skip the current music.")]
        public async Task Skip()
        {
            try
            {
                var msg = Musics.Skip(((SocketGuildUser)Context.User).VoiceChannel);
                await this.RespondAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        [SlashCommand("shuffle", "Toggle shuffle.")]
        public async Task Shuffle()
        {
            try
            {
                var msg = Musics.Shuffle(((SocketGuildUser)Context.User).VoiceChannel);
                await this.RespondAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }

        [SlashCommand("loop", "Switch the mode of loop.")]
        public async Task Loop()
        {
            try
            {
                var msg = Musics.Loop(((SocketGuildUser)Context.User).VoiceChannel);
                await this.RespondAsync(msg);
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


        [SlashCommand("volume", "Change the volume.")]
        public async Task Volume(int volume = 0)
        {
            try
            {
                var msg = Musics.Volume(((SocketGuildUser)Context.User).VoiceChannel, volume);
                await this.RespondAsync(msg);
            }
            catch (Exception e)
            {
                await this.ReplyAsync(e.Message);
                throw;
            }
        }



        public Assembly? GetAssembly()
        {
            return Assembly.GetAssembly(typeof(MusicModule));
        }
    }
}
