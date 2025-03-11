using System.Collections;

namespace RecruitmentAgency.Domain;

public class Qualification
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid? ProfessionId { get; set; }
    public Profession? Profession { get; set; }
    
    public ICollection<Employee>? Employees { get; set; }
    public ICollection<Vacancy>? Vacancies { get; set; }
}