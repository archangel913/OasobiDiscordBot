using Discord;
using Discord.Audio;
using Domain.Interface;

namespace Infrastructure.Discord
{
    internal class AudioSender : IAudioSender
    {

        private IAudioClient? Client { get; set; }

        private AudioOutStream? AudioOutStream { get; set; }

        private readonly SemaphoreSlim Semaphore = new(1);

        public bool IsConnect
            => Client?.ConnectionState == ConnectionState.Connected || Client?.ConnectionState == ConnectionState.Connecting;

        public async Task ConnectAsync(IVoiceChannel channel)
        {
            this.Semaphore.Wait();
            this.Client = await channel.ConnectAsync();
            AudioOutStream = Client.CreatePCMStream(AudioApplication.Music);
            this.Semaphore.Release();
        }

        public async Task UpdateAsync(IVoiceChannel channel)
        {
            this.Semaphore.Wait();
            if (channel is null) throw new NullReferenceException("channel is null.");
            if (this.Client is null) throw new NullReferenceException("Client is null.");
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            this.AudioOutStream.Close();
            this.Client.Dispose();
            this.Client = await channel.ConnectAsync();
            this.AudioOutStream = this.Client.CreatePCMStream(AudioApplication.Music);
            this.Semaphore.Release();
        }

        public async Task DisconnectAsync()
        {
            this.Semaphore.Wait();
            if (this.Client is null) throw new NullReferenceException("Client is null.");
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.Client.StopAsync();
            this.AudioOutStream.Close();
            this.Client.Dispose();
            this.Semaphore.Release();
        }

        public async Task SendMusic(byte[] buffer, int offset, int count)
        {
            this.Semaphore.Wait();
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.AudioOutStream.WriteAsync(buffer.AsMemory(offset, count));
            this.Semaphore.Release();
        }

        public async Task FlushAsync()
        {
            this.Semaphore.Wait();
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.AudioOutStream.FlushAsync().ConfigureAwait(false);
            this.Semaphore.Release();
        }
    }
}
