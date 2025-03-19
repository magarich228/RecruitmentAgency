using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class NewVacancyControl : UserControl
{
    private readonly IServiceProvider _serviceProvider;

    private readonly NewVacancyViewModel _vm;
    private readonly UserControl _prevUserControl;

    public NewVacancyControl()
    {
        InitializeComponent();
    }

    public NewVacancyControl(IServiceProvider serviceProvider, UserControl prev) : this()
    {
        _serviceProvider = serviceProvider;
        _vm = new NewVacancyViewModel(serviceProvider);
        _prevUserControl = prev;

        DataContext = _vm;
    }

    private async void CreateVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            await _vm.CreateVacancy();
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Создание вакансии",
                    ex.Message,
                    ButtonEnum.Ok,
                    Icon.Error)
                .ShowAsPopupAsync(this);

            return;
        }

        await MessageBoxManager.GetMessageBoxStandard(
                "Создание вакансии",
                "Вакансия успешно создана",
                ButtonEnum.Ok,
                Icon.Success)
            .ShowAsPopupAsync(this);
        
        var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainVm.WindowContent = _prevUserControl;

        if (mainVm.WindowContent is VacanciesControl vacanciesControl &&
            vacanciesControl.DataContext is VacanciesViewModel vacanciesViewModel)
        {
            vacanciesViewModel.SearchVacancies();
        }
    }

    private void CancelVacancy_OnClick(object? sender, RoutedEventArgs e)
    {
        var mainVm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainVm.WindowContent = _prevUserControl;
    }
}