using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgency.Api.Dtos;

public class LoginRequest
{
    [Required] public required string PhoneNumber { get; set; }
    [Required] public required string Password { get; set; }
}