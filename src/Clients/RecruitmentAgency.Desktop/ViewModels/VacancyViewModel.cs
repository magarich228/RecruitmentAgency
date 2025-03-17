using System;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class VacancyViewModel : ViewModelBase
{
    public VacancyViewModel(IServiceProvider serviceProvider, VacancyDto vacancy)
    {
        var userContainer = serviceProvider.GetRequiredService<UserContainer>();
        
        Vacancy = vacancy;
        IsEmployee = userContainer.Auth?.Role == "Employee";
    }
    
    public VacancyDto? Vacancy { get; set; }
    
    public bool IsEmployee { get; }
}