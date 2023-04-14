using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Application.Bots;
using Application.Interface;
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
            this.YoutubeToken = "";
            this.DiscordToken = "";
            this.SaveTokenCmd = new SaveSettingsCmd(parent, serviceProvider.GetRequiredService<ISettingsRepository>());
        }

        public CommandBase SaveTokenCmd { get; }

        public string YoutubeToken { get; set; }

        public string DiscordToken { get; set; }
    }
}