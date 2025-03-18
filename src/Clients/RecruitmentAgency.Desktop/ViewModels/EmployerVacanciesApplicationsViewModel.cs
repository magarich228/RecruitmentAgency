using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Models;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class EmployerVacanciesApplicationsViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    
    public EmployerVacanciesApplicationsViewModel()
    {
        
    }

    public EmployerVacanciesApplicationsViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        using var scope = serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();

        var applications = client.ListForEmployerAsync()
            .Result
            .Select(a => new EmployerJobApplicationModel()
            {
                Application = a
            });
        
        VacancyApplications = new ObservableCollection<EmployerJobApplicationModel>(applications);
    }
    
    public ObservableCollection<EmployerJobApplicationModel> VacancyApplications { get; set; }

    public void CreateOffer()
    {
        using var scope = _serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();
        
        
    }
}