using System;
using System.Configuration;
using Application.Interface;
using Application.Settings;
using Newtonsoft.Json.Linq;
using VideoLibrary;
using Version = Application.Settings.Version;

namespace Infrastructure.LocalFile
{
    public class SettingsReader : ISettingsReader
    {
        public bool TryGetSettings(out BotSettings botSettings)
        {
            bool canRead = this.TryReadConfig("GuildId", out var guildIdString);

            var guildIds = new List<GuildId>();
            foreach (var idString in guildIdString.Split(','))
            {
                if (!ulong.TryParse(idString, out ulong id))
                {
                    botSettings = new BotSettings();
                    return false;
                }
                guildIds.Add(new GuildId(id));
            }
            canRead &= this.TryReadConfig("BotName", out string name);
            canRead &= this.TryReadConfig("BotLanguage", out string language);
            canRead &= this.TryReadConfig("DiscordToken", out string token);
            canRead &= this.TryReadConfig("YouTubeApiKey", out string youtubeApiKey);
            canRead &= this.TryReadConfig("BotVersion", out string version);
            if (!canRead)
            {
                botSettings = new BotSettings();
                return false;
            }
            botSettings = new BotSettings()
            {
                BotName = name,
                Version = new Version(version),
                BotLanguage = language,
                DiscordToken = token,
                YouTubeApiKey = youtubeApiKey,
                GuildIds = guildIds
            };
            return true;
        }

        public bool TryGetExperimentalSettings(out BotSettings botSettings)
        {
            string? discordToken = Environment.GetEnvironmentVariable("EXPERIMENTAL_DISCORD_BOT_TOKEN");
            string? youtubeKey = Environment.GetEnvironmentVariable("EXPERIMENTAL_YOUTUBE_API_KEY");
            if (discordToken is null ||
                youtubeKey is null) 
            {
                botSettings = new BotSettings();
                return false;
            }
            botSettings = new BotSettings()
            {
                BotName = "ExperimentOasobiDiscordBot",
                Version = new Version("0.0.0"),
                BotLanguage = "English",
                DiscordToken = discordToken,
                YouTubeApiKey = youtubeKey,
                GuildIds = new List<GuildId>() { new GuildId(0) }
            };
            return true;
        }

        private bool TryReadConfig(string key, out string result)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting is null) 
            {
                result = "";
                return false; 
            }
            result = setting;
            return true;
        }
    }
}
