using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.ViewModels.Commands.MainWindows
{
    abstract class CommandBase : ICommand
    {
        public CommandBase(MainWindowVM mainWindowVM) 
        { 
            this.MainWindowVM = mainWindowVM;
        }

        public event EventHandler? CanExecuteChanged;

        protected MainWindowVM MainWindowVM { get; }

        public bool CanExecute(object? parameter)
            => true;

        public abstract void Execute(object? parameter);
    }
}
