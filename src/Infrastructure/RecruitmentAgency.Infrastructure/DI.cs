using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Application;
using RecruitmentAgency.RelationalData;

namespace RecruitmentAgency.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(Constants.NgpSqlDbConnectionStringKey) ??
                               throw new RecruitmentAgencyInfrastructureException("Connection string not found.");

        services.AddDbContext<RecruitmentDbContext>(options =>
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly("RecruitmentAgency.RelationalData")));
        services.AddScoped<IRecruitmentAgencyContext>(provider => 
            provider.GetRequiredService<RecruitmentDbContext>());
        
        return services;
    }
}