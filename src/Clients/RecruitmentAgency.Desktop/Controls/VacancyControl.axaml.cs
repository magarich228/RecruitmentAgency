using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.ViewModels;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.Controls;

public partial class VacancyControl : UserControl
{
    private readonly VacancyViewModel _vm;
    private readonly UserControl _prevUserControl;
    private readonly IServiceProvider _serviceProvider;
    
    public VacancyControl()
    {
        InitializeComponent();
    }
    
    public VacancyControl(IServiceProvider serviceProvider, UserControl prev, VacancyDto vacancy) : this()
    {
        _vm = new(serviceProvider, vacancy);
        
        _prevUserControl = prev;
        _serviceProvider = serviceProvider;
        
        DataContext = _vm;
    }

    private void CancelVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        
        mainVm.WindowContent = _prevUserControl;
    }
}