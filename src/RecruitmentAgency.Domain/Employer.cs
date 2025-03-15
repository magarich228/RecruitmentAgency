namespace RecruitmentAgency.Domain;

public class Employer : User
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string MainAddress { get; set; }

    public ICollection<Activity>? Activities { get; set; }
    public ICollection<Vacancy>? Vacancies { get; set; }
}