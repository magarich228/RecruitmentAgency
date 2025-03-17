using System;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class VacancyViewModel : ViewModelBase
{
    public VacancyViewModel(IServiceProvider serviceProvider, VacancyDto vacancy)
    {
        Vacancy = vacancy;
    }
    
    public VacancyDto? Vacancy { get; set; }
}