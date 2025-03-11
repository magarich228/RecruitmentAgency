using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.RelationalData.Configuration;

public class JobOfferConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.HasOne(jo => jo.JobApplication)
            .WithOne(ja => ja.JobOffer);
        
        builder.Property(jo => jo.Verdict)
            .HasConversion(ov => ov.ToString(), 
                ov => Enum.Parse<OfferVerdict>(ov));
    }
}