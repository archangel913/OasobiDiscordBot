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
    internal class CloseSettingsSnackBarCmd : CommandBase
    {
        public CloseSettingsSnackBarCmd(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }

        public override void Execute(object? parameter)
        {
            MainWindowVM.SettingsVM.IsSettingsUpdated = false;
        }
    }
}