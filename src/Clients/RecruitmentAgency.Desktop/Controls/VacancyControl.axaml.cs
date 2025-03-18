using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RecruitmentAgency.Desktop.ViewModels;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.Controls;

public partial class VacancyControl : UserControl
{
    private readonly VacancyViewModel _vm;

    private readonly UserControl _prevUserControl;
    private readonly IServiceProvider _serviceProvider;
    private readonly Client _client;

    public VacancyControl()
    {
        InitializeComponent();
    }

    public VacancyControl(IServiceProvider serviceProvider, UserControl prev, VacancyDto vacancy) : this()
    {
        _vm = new(serviceProvider, vacancy);

        _prevUserControl = prev;
        _client = serviceProvider.GetRequiredService<Client>();
        _serviceProvider = serviceProvider;

        DataContext = _vm;
    }

    private void CancelVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();

        mainVm.WindowContent = _prevUserControl;
    }

    private async void ApplyVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        var vacancyId = _vm.Vacancy!.Id;

        try
        {
            await _client.ApplicationAsync(vacancyId);
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Создание отклика",
                    ex.Message,
                    ButtonEnum.Ok,
                    Icon.Error)
                .ShowAsPopupAsync(this);

            return;
        }

        await MessageBoxManager.GetMessageBoxStandard(
                "Создание отклика",
                "Отклик успешно создан",
                ButtonEnum.Ok,
                Icon.Success)
            .ShowAsPopupAsync(this);
    }
}