namespace RecruitmentAgency.Api.Dtos;

public class AuthResponse
{
    public required string Token { get; set; }
    public required string Role { get; set; }
    public DateTime Expiration { get; set; }
}