using Discord;
using Discord.Audio;
using Domain.Interface;

namespace Infrastructure.Discord
{
    internal class AudioSender : IAudioSender
    {

        private IAudioClient? Client { get; set; }

        private IVoiceChannel? VoiceChannel { get; set; }

        private AudioOutStream? AudioOutStream { get; set; }

        public bool IsConnect
            => Client?.ConnectionState == ConnectionState.Connected || Client?.ConnectionState == ConnectionState.Connecting;

        public async Task ConnectAsync(IVoiceChannel channel)
        {
            this.VoiceChannel = channel;
            this.Client = await channel.ConnectAsync();
            AudioOutStream = Client.CreatePCMStream(AudioApplication.Music);
        }

        public async Task UpdateAsync()
        {
            if (this.VoiceChannel is null) throw new NullReferenceException("channel is null.");
            if (this.Client is null) throw new NullReferenceException("Client is null.");
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            this.AudioOutStream.Close();
            this.Client.Dispose();
            this.Client = await this.VoiceChannel.ConnectAsync();
            this.AudioOutStream = this.Client.CreatePCMStream(AudioApplication.Music);
        }

        public async Task DisconnectAsync()
        {
            if (this.Client is null) throw new NullReferenceException("Client is null.");
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.Client.StopAsync();
            this.AudioOutStream.Close();
            this.Client.Dispose();
        }

        public async Task SendMusic(byte[] buffer, int offset, int count)
        {
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.AudioOutStream.WriteAsync(buffer.AsMemory(offset, count));
        }

        public async Task FlushAsync()
        {
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.AudioOutStream.FlushAsync().ConfigureAwait(false);
        }
    }
}
