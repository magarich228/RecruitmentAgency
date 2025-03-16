namespace RecruitmentAgency.Domain;

public class JobOffer
{
    public required Guid Id { get; set; }
    public required Guid VacancyId { get; set; }
    public Guid? JobApplicationId { get; set; }
    public string? Message { get; set; }
    public required OfferVerdict Verdict { get; set; } // Запрет оформления приглашения работодателем работника без его отклика со статусом "отказ"
    
    public required DateTime CreatedDate { get; set; }
    public Vacancy? Vacancy { get; set; }
    public Employee? Employee { get; set; }
    public JobApplication? JobApplication { get; set; }
}

public enum OfferVerdict : int
{
    Invitation = 0,
    Rejection = 1
}