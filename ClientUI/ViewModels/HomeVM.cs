using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Application.Bots;
using ClientUI.Models;
using ClientUI.ViewModels.Commands.MainWindows;
using Microsoft.Extensions.DependencyInjection;

namespace ClientUI.ViewModels
{
    internal class HomeVM : ViewModelBase
    {   
        public HomeVM(MainWindowVM parent, IServiceProvider serviceProvider, ILogPrintable logPrintable)
        {
            this.BotClient = serviceProvider.GetRequiredService<IAsyncBotClient>();
            this.StartBotCmd = new StartBotCommand(parent, this.BotClient, logPrintable);
            
            this.StopBotCmd = new StopCommand(parent, this.BotClient);
            this.OperatingTimer = new Timer(1000);
            this.OperatingTimer.Elapsed += Elapsed;
        }

        private IAsyncBotClient BotClient { get; }

        private OperatingTime operatingTime = new();

        internal OperatingTime OperatingTime
        {
            get
            {
                return this.operatingTime;
            }
            set
            {
                this.operatingTime = value;
                OnPropertyChanged(nameof(this.OperatingTime));
                OnPropertyChanged(nameof(this.OperatingTimeStr));
            }
        }

        public Timer OperatingTimer { get; }

        public CommandBase StartBotCmd { get; }

        public CommandBase StopBotCmd { get; }

        public string OperatingTimeStr
        {
            get
            {
                return this.OperatingTime.ToString();
            }
        }

        // 一秒経過ごとに実行されるメソッド（稼働時間をインクリメントする）
        private void Elapsed(object? timer, EventArgs e)
        {
            this.OperatingTime.Increment();
            this.OnPropertyChanged(nameof(this.OperatingTimeStr));
        }

        ~HomeVM()
        {
            this.OperatingTimer.Dispose();
        }
    }
}