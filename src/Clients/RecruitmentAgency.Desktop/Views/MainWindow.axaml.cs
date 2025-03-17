using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Controls;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _vm;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(IServiceProvider services) : this()
    {
        var vm = services.GetRequiredService<MainWindowViewModel>();

        vm.AuthControl = services.GetRequiredService<AuthControl>();
        
        DataContext = vm;
        _vm = vm;
    }
}