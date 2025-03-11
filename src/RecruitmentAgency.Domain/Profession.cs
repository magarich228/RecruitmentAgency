namespace RecruitmentAgency.Domain;

public class Profession
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Qualification>? AvailableQualifications { get; set; }
}