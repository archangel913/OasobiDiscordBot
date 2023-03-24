using Discord.WebSocket;

namespace Application.Bots
{
    public interface IAsyncBotClient
    {
        public Task StartAsync(ILogPrintable printable);

        public Task StopAsync();

        public Task RestartAsync(ILogPrintable printable);
    }
}
