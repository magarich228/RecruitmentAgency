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
    private readonly IServiceProvider _serviceProvider;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(IServiceProvider services) : this()
    {
        _serviceProvider = services;
        var vm = services.GetRequiredService<MainWindowViewModel>();

        vm.AuthControl = services.GetRequiredService<AuthControl>();
        
        DataContext = vm;
        _vm = vm;
    }

    private void GoToVacancies_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_vm.AuthControl != null) 
            _vm.AuthControl.IsVisible = true;
        
        _vm.WindowContent = _serviceProvider.GetRequiredService<VacanciesControl>();
    }

    private void GoToApplications_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_vm.AuthControl != null)
            _vm.AuthControl.IsVisible = true;
        
        _vm.WindowContent = _serviceProvider.GetRequiredService<JobApplicationsControl>();
    }

    private void GoToVacanciesApplications_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_vm.AuthControl != null)
            _vm.AuthControl.IsVisible = true;
        
        _vm.WindowContent = _serviceProvider.GetRequiredService<EmployerVacanciesApplicationsControl>();
    }
}