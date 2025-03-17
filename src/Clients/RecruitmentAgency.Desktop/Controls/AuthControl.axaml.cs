using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class AuthControl : UserControl
{
    private readonly MainWindowViewModel _vm;
    private readonly LoginControl _loginControl;
    
    public AuthControl()
    {
        InitializeComponent();
    }

    public AuthControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<MainWindowViewModel>();
        _loginControl = serviceProvider.GetRequiredService<LoginControl>();
        
        DataContext = _vm;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        _vm.WindowContent = _loginControl;
        
        if (_vm.AuthControl is not null)
            _vm.AuthControl.IsVisible = false;
    }
}