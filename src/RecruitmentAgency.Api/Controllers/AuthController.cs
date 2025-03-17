using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentAgency.Api.Dtos;
using RecruitmentAgency.Application;

namespace RecruitmentAgency.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await userService.AuthenticateAsync(request);
            return Ok(response);
        }
        catch (RecruitmentAgencyApplicationException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
    
    [HttpPost("adminTest")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> AdminTest()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpPost("employerTest")]
    [Authorize(Roles = "Employer")]
    public Task<IActionResult> EmployerTest()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpPost("employeeTest")]
    [Authorize(Roles = "Employee")]
    public Task<IActionResult> EmployeeTest()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}