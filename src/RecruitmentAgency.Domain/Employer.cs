namespace RecruitmentAgency.Domain;

public class Employer : User
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string MainAddress { get; set; }
    public required IEnumerable<Guid> ActivityIds { get; set; }

    public IEnumerable<Activity>? Activities { get; set; }
}