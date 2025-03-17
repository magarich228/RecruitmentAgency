using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class VacanciesViewModel : ViewModelBase
{
    private readonly Client _client;
    
    public VacanciesViewModel(IServiceProvider serviceProvider)
    {
        _client = serviceProvider.GetRequiredService<Client>();
        
        Vacancies = new();
        MaxSalary = 99999999;
        
        SearchVacancies();
    }

    public ObservableCollection<VacancyDto> Vacancies { get; set; }
    
    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private double _minSalary;
    
    [ObservableProperty]
    private double _maxSalary;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchText) ||
            e.PropertyName == nameof(MinSalary) ||
            e.PropertyName == nameof(MaxSalary))
        {
            SearchVacancies();
        }
        
        base.OnPropertyChanged(e);
    }

    private void SearchVacancies()
    {
        var vacancies = _client
            .SearchAsync(MinSalary, MaxSalary, SearchText)
            .Result;
        
        Vacancies.ReplaceWith(vacancies);
    }
}