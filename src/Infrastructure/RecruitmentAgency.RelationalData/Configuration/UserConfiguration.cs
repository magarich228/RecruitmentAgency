using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.RelationalData.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // builder.HasDiscriminator<string>("UserRole")
        //     .HasValue<Admin>("Admin")
        //     .HasValue<Employer>("Employer")
        //     .HasValue<Employee>("Employee");
    }
}