using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class NewVacancyViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    
    public NewVacancyViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        Vacancy = new();
    }
    
    public VacancyCreateDto Vacancy { get; set; }
    
    public async Task CreateVacancy()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();

        _ = await client.CreateAsync(Vacancy);
    }
}