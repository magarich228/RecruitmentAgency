﻿using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Controls;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly UserContainer _userContainer;
    private readonly IServiceProvider _serviceProvider;
    
    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();
        _userContainer.AuthChanged += UserContainerOnAuthChanged;

        _windowContent = serviceProvider.GetRequiredService<VacanciesControl>();
    }
    
    [ObservableProperty]
    private UserControl? _authControl;
    
    [ObservableProperty]
    private UserControl? _windowContent;

    [ObservableProperty] 
    private bool _isApplicationsVisible;
    
    [ObservableProperty]
    private bool _isVacanciesApplicationsVisible;

    [ObservableProperty] 
    private bool _isReportVisible;
    
    private void UserContainerOnAuthChanged(object? sender, AuthResponse? e)
    {
        if (AuthControl is not null)
            AuthControl.IsVisible = true;
        
        if (e is null)
        {
            AuthControl = _serviceProvider.GetRequiredService<AuthControl>();
            WindowContent = _serviceProvider.GetRequiredService<VacanciesControl>();

            IsApplicationsVisible = false;
            IsVacanciesApplicationsVisible = false;
            IsReportVisible = false;
        }
        else
        {
            var miniProfileControl = _serviceProvider.GetRequiredService<MiniProfileControl>();
            var miniProfileVm = _serviceProvider.GetRequiredService<MiniProfileViewModel>();

            miniProfileVm.Refresh();
            
            AuthControl = miniProfileControl;

            switch (e.Role)
            {
                case "Employee":
                    IsApplicationsVisible = true;
                    break;
                
                case "Employer":
                    IsVacanciesApplicationsVisible = true;
                    break;
                
                case "Admin":
                    IsReportVisible = true;
                    break;
                
                default:
                    throw new ApplicationException("Unknown role");
            }
        }
    }
}