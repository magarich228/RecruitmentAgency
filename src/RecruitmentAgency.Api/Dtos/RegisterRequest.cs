using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgency.Api.Dtos;

public abstract class RegisterRequest
{
    [Required] 
    public required string PhoneNumber { get; set; }
    
    [Required] 
    public required string Password { get; set; }
    
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают.")]
    public required string ConfirmPassword { get; set; }
}

public class RegisterEmployeeRequest : RegisterRequest
{
    [Required]
    public required string FullName { get; set; }
    
    public string? Resume { get; set; }
}

public class RegisterEmployerRequest : RegisterRequest
{
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public required string MainAddress { get; set; }
}