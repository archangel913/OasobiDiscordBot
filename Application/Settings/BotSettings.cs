using Application.Interface;
using Domain.Factory;

namespace Application.Settings
{
    public class BotSettings
    {
        public string BotName { get; init; }

        public Version Version { get; init; }

        public string BotLanguage { get; init; }

        public string DiscordToken { get; init; }

        public List<GuildId> GuildIds { get; init; } = new List<GuildId>();

        public string YouTubeApiKey { get; init; }
    }
}