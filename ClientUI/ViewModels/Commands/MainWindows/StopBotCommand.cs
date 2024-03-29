﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bots;
using Application.Musics;

namespace ClientUI.ViewModels.Commands.MainWindows
{
    internal class StopCommand : CommandBase
    {
        public StopCommand(MainWindowVM mainWindowVM, IAsyncBotClient botClient) : base(mainWindowVM)
        {
            this.BotClient = botClient;
        }

        private IAsyncBotClient BotClient { get; }

        public override void Execute(object? parameter)
        {
            MusicPlayerProvider.Clear();
            this.BotClient.StopAsync();
            this.MainWindowVM.HomeVM.OperatingTimer.Stop();
            this.MainWindowVM.HomeVM.OperatingTime = new Models.OperatingTime();
        }
    }
}
