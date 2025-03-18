using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RecruitmentAgency.Desktop.Models;
using RecruitmentAgency.Desktop.ViewModels;
using RecruitmentAgency.HttpClient;

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
        CreateOffer(sender, OfferVerdict._0);
    }

    private void RejectOffer_OnClick(object? sender, RoutedEventArgs e)
    {
        CreateOffer(sender, OfferVerdict._1);
    }

    private void CreateOffer(object? sender, OfferVerdict verdict)
    {
        var button = sender as Button ?? throw new InvalidOperationException("Offer button not found");
        
        var model = button.DataContext as EmployerJobApplicationModel ?? throw new InvalidOperationException("Job application not found");

        try
        {
            _vm.CreateOffer(model.Application.Id, model.Application.EmployeeId, verdict);
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard(
                "Отправка ответа на отклик",
                ex.Message,
                ButtonEnum.Ok,
                Icon.Error).ShowAsPopupAsync(this);

            return;
        }

        MessageBoxManager.GetMessageBoxStandard(
            "Отправка ответа на отклик",
            "Ваш ответ успешно отправлен!",
            ButtonEnum.Ok,
            Icon.Success
        ).ShowAsPopupAsync(this);
    }
}