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
        
        return services;
    }
}