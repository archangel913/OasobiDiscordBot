using System;
using System.Configuration;
using Application.Interface;
using Application.Settings;
using Version = Application.Settings.Version;

namespace Infrastructure.LocalFile
{
    internal class SettingsReader : ISettingsReader
    {
        public BotSettings GetSettings()
        {
            var guildIdStrings = this.GetAppConfig("GuildId").Split(",");
            var guildIds = guildIdStrings.Select(id => new GuildId(ulong.Parse(id))).ToList();
            var settings = new BotSettings()
            {
                BotName = this.GetAppConfig("BotName"),
                Version = this.CreateVersion(),
                BotLanguage = this.GetAppConfig("BotLanguage"),
                DiscordToken = this.GetAppConfig("DiscordToken"),
                GuildIds = guildIds
            };
            return settings;
        }

        private Version CreateVersion()
        {
            string version = this.GetAppConfig("BotVersion");
            var splitedVersion = version.Split('.');
            return new Version()
            {
                Major = int.Parse(splitedVersion[0]),
                Minor = int.Parse(splitedVersion[1]),
                Patch = int.Parse(splitedVersion[2])
            };
        }

        private string GetAppConfig(string key)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting is null) throw new KeyNotFoundException($"not found {key} setting in app.config");
            return setting;
        }
    }
}
