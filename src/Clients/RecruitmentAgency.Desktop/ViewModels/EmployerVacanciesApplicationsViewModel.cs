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
        
        Refresh();
    }
    
    public ObservableCollection<EmployerJobApplicationModel> VacancyApplications { get; set; }

    public void CreateOffer(Guid applicationId, string employeeId, OfferVerdict verdict)
    {
        using var scope = _serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();

        var body = new CreateOfferDto()
        {
            ApplicationId = applicationId,
            EmployeeId = employeeId,
            Message = verdict == OfferVerdict._0 ? "Мы с вами свяжемся!" : "Извините, но мы не можем вас пригласить",
            Verdict = verdict
        };
        
        client.OfferAsync(body)
            .Wait();
        
        Refresh();
    }

    private void Refresh()
    {
        using var scope = _serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();

        var applications = client.ListForEmployerAsync()
            .Result
            .Select(a => new EmployerJobApplicationModel()
            {
                Application = a
            });
        
        VacancyApplications = new ObservableCollection<EmployerJobApplicationModel>(applications);
    }
}