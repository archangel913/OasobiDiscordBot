using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ClientUI.Views;

namespace ClientUI;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public App(IServiceProvider service)
    {
        this.Service = service;
    }

    private IServiceProvider Service { get; }

    public void Start()
    {
        InitializeComponent();
        Run();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var main = new MainWindow(this.Service);
        main.Show();
    }
}
