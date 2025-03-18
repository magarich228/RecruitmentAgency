using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Models;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class JobApplicationsViewModel : ViewModelBase
{
    public JobApplicationsViewModel(IServiceProvider serviceProvider)
    {
        var client = serviceProvider.GetRequiredService<Client>();

        var result = client.ListAsync()
            .Result
            .Select(a => new JobApplicationModel()
            {
                Application = a
            });
        
        Applications = new(result);
    }
    
    public ObservableCollection<JobApplicationModel> Applications { get; set; }
}