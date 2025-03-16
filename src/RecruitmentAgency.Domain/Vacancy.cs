namespace RecruitmentAgency.Domain;

public class Vacancy
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public required decimal Commission { get; set; }
    public required DateTime CreationDate { get; set; }
    
    public ICollection<Qualification>? Qualifications { get; set; }
    public Employer? Employer { get; set; }
    
    public ICollection<JobOffer>? JobOffers { get; set; }
    public ICollection<JobApplication>? JobApplications { get; set; }
}