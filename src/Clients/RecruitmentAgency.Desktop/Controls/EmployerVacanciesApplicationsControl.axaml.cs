using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class EmployerVacanciesApplicationsControl : UserControl
{
    private readonly EmployerVacanciesApplicationsViewModel _vm;
    
    public EmployerVacanciesApplicationsControl()
    {
        InitializeComponent();
    }

    public EmployerVacanciesApplicationsControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<EmployerVacanciesApplicationsViewModel>();

        DataContext = _vm;
    }

    private void Offer_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void RejectOffer_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}