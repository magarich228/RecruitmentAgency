using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgency.Api.Dtos;

public class LoginRequest
{
    [Required] public string PhoneNumber { get; set; }
    [Required] public string Password { get; set; }
}