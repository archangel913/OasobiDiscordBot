using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application.Bots;
using ClientUI.ViewModels;

namespace ClientUI.Views;
/// <summary>
/// MainWindow.xaml の相互作用ロジック
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        var vm = new MainWindowVM(serviceProvider, new LogPrinter(this));
        this.DataContext = vm;
    }
}
