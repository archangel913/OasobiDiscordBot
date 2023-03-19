using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Application.Bots;

namespace ClientUI.Views
{
    class LogPrinter : ILogPrintable
    {
        public LogPrinter(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
        }

        private MainWindow MainWindow { get; }

        public void PrintCommandError(string header, string performer, string message)
        {
            this.MainWindow.Dispatcher.Invoke(() => {
                var textBox = MainWindow.logTextBox;
                var redBrush = Brushes.Red;
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run()
                {
                    Text = header,
                    Foreground = redBrush
                });
                paragraph.Inlines.Add(new Run()
                {
                    Text = performer,
                    Foreground = redBrush
                });
                paragraph.Inlines.Add(new Run()
                {
                    Text = message,
                    Foreground = redBrush
                });
                textBox.Document.Blocks.Add(paragraph);
            });
            
        }

        public void PrintCommandSuccess(string header, string performer, string message)
        {
            this.MainWindow.Dispatcher.Invoke(() => {
                var textBox = MainWindow.logTextBox;
                var greenBrush = Brushes.GreenYellow;
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run()
                {
                    Text = header,
                    Foreground = greenBrush
                });
                paragraph.Inlines.Add(new Run()
                {
                    Text = performer,
                    Foreground = greenBrush
                });
                paragraph.Inlines.Add(new Run()
                {
                    Text = message,
                    Foreground = greenBrush
                });
                textBox.Document.Blocks.Add(paragraph);
            });
        }

        public void PrintError(string header, string errorMessage)
        {
            this.MainWindow.Dispatcher.Invoke(() => {
                var textBox = MainWindow.logTextBox;
                var redBrush = Brushes.Red;
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run()
                {
                    Text = header,
                    Foreground = redBrush
                });
                paragraph.Inlines.Add(new Run()
                {
                    Text = errorMessage,
                    Foreground = redBrush
                });
                textBox.Document.Blocks.Add(paragraph);
            });
            
        }

        public void PrintNormalLog(string header, string message)
        {
            this.MainWindow.Dispatcher.Invoke(() =>
            {
                var textBox = MainWindow.logTextBox;
                var yellow = Brushes.Gold;
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run()
                {
                    Text = header,
                    Foreground = yellow
                });
                paragraph.Inlines.Add(new Run()
                {
                    Text = message,
                    Foreground = Brushes.White
                });
                textBox.Document.Blocks.Add(paragraph);
            });
        }

        public void PrintSystemLog(string header, string message)
        {
            PrintNormalLog(header, message);
        }
    }
}
