using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Application.Bots;
using Application.Interface;
using Application.Settings;
using ClientUI.Models;
using ClientUI.ViewModels.Commands.MainWindows;
using Discord.Rest;
using Microsoft.Extensions.DependencyInjection;

namespace ClientUI.ViewModels
{
    internal class SettingsVM : ViewModelBase
    {
        public SettingsVM(MainWindowVM parent, IServiceProvider serviceProvider)
        {
            var settingsRepository = serviceProvider.GetRequiredService<ISettingsRepository>();

            BotSettings botSettings;
            settingsRepository.TryGetSettings(out botSettings);

            this.YoutubeToken = botSettings.YouTubeToken;
            this.DiscordToken = botSettings.DiscordToken;
            this.SaveTokenCmd = new SaveSettingsCmd(parent, settingsRepository);
            this.CloseSettingsSnackBarCmd = new CloseSettingsSnackBarCmd(parent);
        }

        public CommandBase SaveTokenCmd { get; }

        public CommandBase CloseSettingsSnackBarCmd { get; }

        public string YoutubeToken { get; set; }

        public string DiscordToken { get; set; }

        private bool isSettingsUpdated = false;
        public bool IsSettingsUpdated
        {
            get => this.isSettingsUpdated;
            set
            {
                this.isSettingsUpdated = value;
                OnPropertyChanged(nameof(this.IsSettingsUpdated));
            }
        }
    }
}