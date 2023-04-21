using Application.Bots;
using Application.Interface;
using Application.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Commands.MainWindows
{
    internal class SaveSettingsCmd : CommandBase
    {
        public SaveSettingsCmd(MainWindowVM mainWindowVM, ISettingsRepository settingsRepository) : base(mainWindowVM)
        {
            this.SettingsRepository = settingsRepository;
        }

        private readonly ISettingsRepository SettingsRepository;

        public override void Execute(object? parameter)
        {
            var newBotSettings = new BotSettings()
            {
                DiscordToken = this.MainWindowVM.SettingsVM.DiscordToken,
                YouTubeToken = this.MainWindowVM.SettingsVM.YoutubeToken,
            };
            this.SettingsRepository.Save(newBotSettings);
        }
    }
}
