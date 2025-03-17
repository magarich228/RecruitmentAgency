using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class VacanciesControl : UserControl
{
    private readonly VacanciesViewModel _vm;
    
    public VacanciesControl()
    {
        InitializeComponent();
    }

    public VacanciesControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<VacanciesViewModel>();
        DataContext = _vm;
    }
}