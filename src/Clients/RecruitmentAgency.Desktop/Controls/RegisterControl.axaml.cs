using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class RegisterControl : UserControl
{
    private readonly RegisterViewModel _vm;
    private readonly MainWindowViewModel _mainVm;
    
    private readonly VacanciesControl _vacanciesControl;
    private readonly MiniProfileControl _miniProfileControl;
    private readonly IServiceProvider _serviceProvider;
    
    public RegisterControl()
    {
        InitializeComponent();
    }
    
    public RegisterControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<RegisterViewModel>();
        _mainVm = serviceProvider.GetRequiredService<MainWindowViewModel>();

        _vacanciesControl = serviceProvider.GetRequiredService<VacanciesControl>();
        _miniProfileControl = serviceProvider.GetRequiredService<MiniProfileControl>();
        _serviceProvider = serviceProvider;
        
        DataContext = _vm;
    }
    
    private void Login_OnClick(object? sender, RoutedEventArgs e)
    {
        _mainVm.WindowContent = _serviceProvider.GetRequiredService<LoginControl>();
    }
    
    private async void Registration_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            await _vm.RegisterAsync();
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Ошибка при регистрации",
                    ex.Message,
                    ButtonEnum.Ok,
                    Icon.Error)
                .ShowAsPopupAsync(this);

            return;
        }

        _mainVm.WindowContent = _vacanciesControl;
        _mainVm.AuthControl!.IsVisible = true;
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        _mainVm.WindowContent = _vacanciesControl;
        _mainVm.AuthControl!.IsVisible = true;
    }
}