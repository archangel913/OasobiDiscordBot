using Discord;
using Discord.WebSocket;
using Application.Interface;

namespace Infrastructure.Discord
{
    internal class Connecter : IDiscordConnecter
    {
        private static DiscordSocketClient? Client;
        private static string? BotToken;
        public async Task ConnectAsync(DiscordSocketClient client, string token)
        {
            Client = client;
            BotToken = token;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }

        public async Task DisconnectAsync(DiscordSocketClient client)
        {
            await client.StopAsync();
            await client.LogoutAsync();
        }

        public async Task Reconnect()
        {
            if (Client is null || BotToken is null) throw new NullReferenceException("Client or BotToken is null in DiscordConnecter");
            await Client.StopAsync();
            await Client.LogoutAsync();
            await Client.LoginAsync(TokenType.Bot, BotToken);
            await Client.StartAsync();
        }
    }
}
