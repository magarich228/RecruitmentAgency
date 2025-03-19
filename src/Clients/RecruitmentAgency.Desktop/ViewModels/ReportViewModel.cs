using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class ReportViewModel : ViewModelBase
{
    private readonly Client _client;
    
    public ReportViewModel()
    {
        
    }

    public ReportViewModel(IServiceProvider serviceProvider)
    {
        _client = serviceProvider.GetRequiredService<Client>();
        
        To = DateTime.Now;
    }
    
    [ObservableProperty]
    private DateTime _from;
    
    [ObservableProperty]
    private DateTime _to;
    
    [ObservableProperty]
    private double _commissionIncome;

    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(From) ||
            e.PropertyName == nameof(To))
        {
            var report = await _client.ReportAsync(From, To);

            CommissionIncome = report.CommissionIncome;
        }
        
        base.OnPropertyChanged(e);
    }
}