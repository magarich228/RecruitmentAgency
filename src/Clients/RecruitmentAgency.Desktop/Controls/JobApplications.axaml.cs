using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.ViewModels;

namespace RecruitmentAgency.Desktop.Controls;

public partial class JobApplicationsControl : UserControl
{
    private readonly JobApplicationsViewModel _vm;
    private readonly IServiceProvider _serviceProvider;
    
    public JobApplicationsControl()
    {
        InitializeComponent();
    }

    public JobApplicationsControl(IServiceProvider serviceProvider) : this()
    {
        _vm = serviceProvider.GetRequiredService<JobApplicationsViewModel>();
        _serviceProvider = serviceProvider;

        DataContext = _vm;
    }
}