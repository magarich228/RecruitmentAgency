using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class VacanciesViewModel : ViewModelBase
{
    private readonly UserContainer _userContainer;
    private readonly Client _client;
    
    public VacanciesViewModel(IServiceProvider serviceProvider)
    {
        _client = serviceProvider.GetRequiredService<Client>();
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();
        
        _userContainer.AuthChanged += (s, a) => OnPropertyChanged(nameof(IsEmployer));
        
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

    public bool IsEmployer
    {
        get
        {
            if (_userContainer.Auth != null)
            {
                return _userContainer.Auth.Role == "Employer";
            }
            
            return false;
        }
    }
    
    [ObservableProperty]
    private bool _isOnlyEmployerVacancies;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchText) ||
            e.PropertyName == nameof(MinSalary) ||
            e.PropertyName == nameof(MaxSalary) ||
            e.PropertyName == nameof(IsEmployer) ||
            e.PropertyName == nameof(IsOnlyEmployerVacancies))
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

        if (IsEmployer && IsOnlyEmployerVacancies)
        {
            vacancies = vacancies
                .Where(v => _userContainer.Id == v.EmployerId)
                .ToList();
        }
        
        Vacancies.ReplaceWith(vacancies);
    }
}