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

namespace ClientUI.ViewModels;
internal class MainWindowVM
{
    public MainWindowVM(IServiceProvider serviceProvider, ILogPrintable logPrintable)
    {
        this.HomeVM = new HomeVM(this, serviceProvider, logPrintable);
        this.SettingsVM = new SettingsVM(this, serviceProvider);
    }

    public HomeVM HomeVM { get; }

    public SettingsVM SettingsVM { get; }
}
