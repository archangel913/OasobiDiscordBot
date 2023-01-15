using Discord.WebSocket;

namespace Application.Interface
{
    public interface IDiscordConnecter
    {
        public Task ConnectAsync(DiscordSocketClient client, string token);
        public Task DisconnectAsync(DiscordSocketClient client);
        public Task Reconnect();
    }
}
