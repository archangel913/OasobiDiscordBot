using System;
using System.Configuration;
using Application.Interface;
using Application.Settings;
using Newtonsoft.Json.Linq;
using Version = Application.Settings.Version;

namespace Infrastructure.LocalFile
{
    public class SettingsRepository : ISettingsRepository
    {
        public bool TryGetSettings(out BotSettings botSettings)
        {
            bool canRead = this.TryReadConfig("BotName", out string name);
            canRead &= this.TryReadConfig("BotLanguage", out string language);
            canRead &= this.TryReadConfig("DiscordToken", out string token);
            canRead &= this.TryReadConfig("BotVersion", out string version);
            if (!canRead || token == "")
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
            };
            return true;
        }

        public bool TryGetExperimentalSettings(out BotSettings botSettings)
        {
            string? discordToken = Environment.GetEnvironmentVariable("EXPERIMENTAL_DISCORD_BOT_TOKEN");
            if (discordToken is null) 
            {
                botSettings = new BotSettings();
                return false;
            }
            botSettings = new BotSettings()
            {
                BotName = "ExperimentOasobiDiscordBot",
                Version = new Version("0.0.0"),
                BotLanguage = "English",
                DiscordToken = discordToken
            };
            return true;
        }

        public void Save(BotSettings newBotSettings)
        {
            //ToDo : YouTubeAPIを追加する
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings["DiscordToken"] != null)
                {
                    settings["DiscordToken"].Value = newBotSettings.DiscordToken;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
