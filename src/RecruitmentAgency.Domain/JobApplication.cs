namespace RecruitmentAgency.Domain;

public class JobApplication
{
    public required Guid Id { get; set; }
    public required Guid VacancyId { get; set; }
    public required DateTime ApplicationDate { get; set; }

    public Vacancy? Vacancy { get; set; }
    public Employee? Employee { get; set; }
    public JobOffer? JobOffer { get; set; }
}