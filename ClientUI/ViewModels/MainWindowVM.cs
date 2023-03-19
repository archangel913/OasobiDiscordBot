using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Bots;
using ClientUI.Models;
using ClientUI.ViewModels.Commands.MainWindows;
using Microsoft.Extensions.DependencyInjection;

namespace ClientUI.ViewModels;
internal class MainWindowVM
{
    public MainWindowVM(IServiceProvider serviceProvider, ILogPrintable logPrintable)
    {
        this.BotClient = serviceProvider.GetRequiredService<IAsyncBotClient>();
        this.StartBotCmd = new StartBotCommand(this, this.BotClient, logPrintable);
    }

    private IAsyncBotClient BotClient { get; }

    private OperatingTime OperatingTime { get; set; } = new OperatingTime();

    public CommandBase StartBotCmd { get; }

    public string OperatingTimeStr { get => this.OperatingTime.ToString(); }
}
