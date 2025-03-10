namespace RecruitmentAgency.Domain;

public class Qualification
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid? ProfessionId { get; set; }
    public Profession? Profession { get; set; }
}