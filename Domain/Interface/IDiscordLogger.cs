using Discord.WebSocket;
using Discord.Interactions;

namespace Domain.Interface
{
    public interface IDiscordLogger
    {
        public abstract void Register(DiscordSocketClient client, InteractionService interactionService);
        public void WriteBotSystemLog(string message, ConsoleColor color = ConsoleColor.Cyan);
    }
}
