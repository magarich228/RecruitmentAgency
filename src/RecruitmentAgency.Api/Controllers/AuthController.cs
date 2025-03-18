using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Api.Dtos;
using RecruitmentAgency.Application;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IUserService userService,
    IRecruitmentAgencyContext db) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(Roles = "Employee, Employer, Admin")]
    public async Task<ActionResult<UserInfo>> MeAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new InvalidOperationException("User not found");

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            throw new InvalidOperationException("User not found");

        var userInfo = new UserInfo()
        {
            Id = user.Id,
            PhoneNumber = user.PhoneNumber
        };

        if (user is Employee emp)
        {
            userInfo.FullName = emp.FullName;
            userInfo.Resume = emp.Resume;
        }

        if (user is Employer emplr)
        {
            userInfo.Name = emplr.Name;
            userInfo.Description = emplr.Description;
            userInfo.MainAddress = emplr.MainAddress;
        }

        return Ok(userInfo);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request) =>
        await LoginInternalAsync(request);

    [HttpPost("registerAsEmployee")]
    public async Task<ActionResult<AuthResponse>> RegisterAsEmployee([FromBody] RegisterEmployeeRequest request)
    {
        var exists = await db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

        if (exists is not null)
            return BadRequest("Пользователь с таким номером уже существует");

        var user = new Employee()
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = request.PhoneNumber,
            PasswordHash = request.Password,
            FullName = request.FullName,
            Resume = request.Resume
        };

        await db.Employees.AddAsync(user);

        await db.SaveChangesAsync();

        return await LoginInternalAsync(new LoginRequest()
        {
            PhoneNumber = request.PhoneNumber,
            Password = request.Password
        });
    }

    [HttpPost("registerAsEmployer")]
    public async Task<ActionResult<AuthResponse>> RegisterAsEmployer([FromBody] RegisterEmployerRequest request)
    {
        var exists = await db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

        if (exists is not null)
            return BadRequest("Пользователь с таким номером уже существует");

        var user = new Employer()
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = request.PhoneNumber,
            PasswordHash = request.Password,
            Name = request.Name,
            Description = request.Description,
            MainAddress = request.MainAddress
        };

        await db.Employers.AddAsync(user);

        await db.SaveChangesAsync();

        return await LoginInternalAsync(new LoginRequest()
        {
            PhoneNumber = request.PhoneNumber,
            Password = request.Password
        });
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

    [NonAction]
    private async Task<ActionResult<AuthResponse>> LoginInternalAsync(LoginRequest request)
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
}