using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Application;
using RecruitmentAgency.Domain;
using RecruitmentAgency.RelationalData.Configuration;

namespace RecruitmentAgency.RelationalData;

public class RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) :
    DbContext(options), IRecruitmentAgencyContext
{
    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Employer> Employers { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<JobApplication> JobApplications { get; set; } = null!;
    public DbSet<JobOffer> JobOffers { get; set; } = null!;
    public DbSet<Profession> Professions { get; set; } = null!;
    public DbSet<Qualification> Qualifications { get; set; } = null!;
    public DbSet<Vacancy> Vacancies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployerConfiguration());
        modelBuilder.ApplyConfiguration(new JobOfferConfiguration());
        modelBuilder.ApplyConfiguration(new ProfessionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new VacancyConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}