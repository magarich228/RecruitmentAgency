namespace RecruitmentAgency.Domain;

public class User
{
    public required string Id { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
    public string? Image { get; set; }
}