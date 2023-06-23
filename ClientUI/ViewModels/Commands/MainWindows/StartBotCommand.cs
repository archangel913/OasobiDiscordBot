using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bots;

namespace ClientUI.ViewModels.Commands.MainWindows
{
    internal class StartBotCommand : CommandBase
    {
        public StartBotCommand(MainWindowVM mainWindowVM, IAsyncBotClient botClient, ILogPrintable logPrintable) : base(mainWindowVM)
        {
            this.BotClient = botClient;
            this.LogPrintable = logPrintable;
        }

        private IAsyncBotClient BotClient { get; }

        private ILogPrintable LogPrintable { get; }

        public override void Execute(object? parameter)
        {
            this.BotClient.StartAsync(this.LogPrintable);
            this.MainWindowVM.HomeVM.OperatingTimer.Start();
        }
    }
}
