using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class MiniProfileControl : UserControl
{
    private readonly MiniProfileViewModel _vm;
    private readonly UserContainer _userContainer;
    
    public MiniProfileControl()
    {
        InitializeComponent();
    }

    public MiniProfileControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<MiniProfileViewModel>();
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();

        DataContext = _vm;
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        _userContainer.Auth = null;
        _userContainer.PhoneNumber = null;
        _userContainer.PasswordHash = null;
    }
}