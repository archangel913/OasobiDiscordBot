using Application.Interface;

namespace Application.Settings
{
    public class BotSettings
    {
        public string BotName { get; init; }

        public Version Version { get; init; }

        public string BotLanguage { get; init; }

        public string DiscordToken { get; init; }
    }
}