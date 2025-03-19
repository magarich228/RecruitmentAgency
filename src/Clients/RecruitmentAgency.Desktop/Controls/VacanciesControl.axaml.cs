using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using RecruitmentAgency.Desktop.ViewModels;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.Controls;

public partial class VacanciesControl : UserControl
{
    private readonly VacanciesViewModel _vm;
    private readonly IServiceProvider _serviceProvider;
    
    public VacanciesControl()
    {
        InitializeComponent();
    }

    public VacanciesControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<VacanciesViewModel>();
        _serviceProvider = serviceProvider;
        
        DataContext = _vm;
    }

    private void OpenVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        if (button is null)
            return;
        
        var vacancy = button.DataContext as VacancyDto;
        
        if (vacancy is null)
            return;

        var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainVm.WindowContent = new VacancyControl(_serviceProvider, this, vacancy);
    }

    private void CreateVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainVm.WindowContent = new NewVacancyControl(_serviceProvider, this);
    }
}