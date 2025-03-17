using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class LoginControl : UserControl
{
    private readonly LoginViewModel _vm;
    private readonly MainWindowViewModel _mainVm;
    
    private readonly VacanciesControl _vacanciesControl;
    private readonly MiniProfileControl _miniProfileControl;
    
    public LoginControl()
    {
        InitializeComponent();
    }

    public LoginControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<LoginViewModel>();
        _mainVm = serviceProvider.GetRequiredService<MainWindowViewModel>();

        _vacanciesControl = serviceProvider.GetRequiredService<VacanciesControl>();
        _miniProfileControl = serviceProvider.GetRequiredService<MiniProfileControl>();
        
        DataContext = _vm;
    }

    private void Registration_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
    
    private async void Login_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            await _vm.AuthorizeAsync();
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Ошибка при входе",
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