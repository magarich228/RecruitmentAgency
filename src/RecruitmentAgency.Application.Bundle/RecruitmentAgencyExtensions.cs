using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Infrastructure;

namespace RecruitmentAgency.Application.Bundle;

public static class RecruitmentAgencyExtensions
{
    public static IServiceCollection AddRecruitmentAgency(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        
        services.AddScoped<IVacancyService, VacancyService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployerService, EmployerService>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();
        services.AddScoped<IQualificationService, QualificationService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IProfessionService, ProfessionService>();
        
        return services;
    }
}