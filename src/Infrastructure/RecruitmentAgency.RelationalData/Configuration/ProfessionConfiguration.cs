using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.RelationalData.Configuration;

public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.HasMany(p => p.AvailableQualifications)
            .WithOne(q => q.Profession);
    }
}