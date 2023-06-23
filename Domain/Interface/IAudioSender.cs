using Discord;

namespace Domain.Interface
{
    public interface IAudioSender
    {
        public Task ConnectAsync(IVoiceChannel channel);
        public Task UpdateAsync();
        public Task DisconnectAsync();
        public Task SendMusic(byte[] buffer, int offset, int count);
        public Task FlushAsync();
        public bool IsConnect { get; }
    }
}
