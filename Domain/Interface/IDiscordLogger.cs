using Discord.WebSocket;
using Discord.Interactions;

namespace Domain.Interface
{
    public interface IDiscordLogger
    {
        public void WriteBotSystemLog(string message);

        public void WriteErrorLog(string message);
    }
}
