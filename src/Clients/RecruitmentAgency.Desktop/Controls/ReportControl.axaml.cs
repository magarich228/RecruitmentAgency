using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class ReportControl : UserControl
{
    private readonly ReportViewModel _vm;
    
    public ReportControl()
    {
        InitializeComponent();
    }

    public ReportControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<ReportViewModel>();
        
        DataContext = _vm;
    }
}