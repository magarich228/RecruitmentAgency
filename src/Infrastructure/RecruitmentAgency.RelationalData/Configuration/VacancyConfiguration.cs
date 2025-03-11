using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.RelationalData.Configuration;

public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.HasOne(v => v.Employer)
            .WithMany(e => e.Vacancies);

        builder.HasMany(v => v.Qualifications)
            .WithMany(q => q.Vacancies);
        
        builder.HasMany(v => v.JobApplications)
            .WithOne(j => j.Vacancy);

        builder.HasMany(v => v.JobOffers)
            .WithOne(o => o.Vacancy);
    }
}