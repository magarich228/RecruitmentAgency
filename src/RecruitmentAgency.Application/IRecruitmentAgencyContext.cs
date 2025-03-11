using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Application;

public interface IRecruitmentAgencyContext
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}