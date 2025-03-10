namespace RecruitmentAgency.Domain;

public class Vacancy
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required IEnumerable<int> QualificationsIds { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public required Guid EmployerId { get; set; }
    public required decimal Commission { get; set; }
    
    public IEnumerable<Qualification>? Qualifications { get; set; }
    public Employer? Employer { get; set; }
    
    public IEnumerable<JobOffer>? JobOffers { get; set; }
    public IEnumerable<JobApplication>? JobApplications { get; set; }
}