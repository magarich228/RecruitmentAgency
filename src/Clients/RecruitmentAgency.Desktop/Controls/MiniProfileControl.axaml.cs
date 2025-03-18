using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.Desktop.ViewModels;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.Controls;

public partial class MiniProfileControl : UserControl
{
    private readonly MiniProfileViewModel _vm;
    private readonly UserContainer _userContainer;
    private readonly IServiceProvider _serviceProvider;
    
    public MiniProfileControl()
    {
        InitializeComponent();
    }

    public MiniProfileControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<MiniProfileViewModel>();
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();
        _serviceProvider = serviceProvider;

        DataContext = _vm;
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        _userContainer.Auth = null;
        _userContainer.PhoneNumber = null;
        _userContainer.PasswordHash = null;

        Client.Token = null;
    }
}