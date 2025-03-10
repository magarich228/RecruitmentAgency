namespace RecruitmentAgency.Domain;

public class Employee : User
{
    public required string FullName { get; set; }
    public string? Resume { get; set; }
    public IEnumerable<Qualification> Qualifications { get; set; }
}