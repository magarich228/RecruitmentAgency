using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Application;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanciesController(
    IVacancyService vacancyService,
    IJobApplicationService jobApplicationService,
    IRecruitmentAgencyContext db) : ControllerBase
{
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<VacancyDto>>> SearchAsync([FromQuery] VacancySearchFilter filter)
    {
        var result = await vacancyService.SearchVacanciesAsync(filter);

        return Ok(result);
    }

    [HttpPost("application")]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult> ApplyAsync([FromBody] Guid vacancyId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new InvalidOperationException("User not found");

        _ = await jobApplicationService.CreateApplicationAsync(userId, vacancyId);

        return Ok();
    }

    [HttpPost("create")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<VacancyDto>> CreateAsync([FromBody] VacancyCreateDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new InvalidOperationException("User not found");

        var currentUser = await db.Employers
            .FirstOrDefaultAsync(u => u.Id == userId) ?? 
                          throw new InvalidOperationException("User not found");
        
        var vacancy = new Vacancy
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            MinSalary = dto.MinSalary,
            MaxSalary = dto.MaxSalary,
            Commission = Random.Shared.Next((int)dto.MaxSalary, (int)dto.MaxSalary),
            Employer = currentUser,
            Qualifications = new List<Qualification>(),
            CreationDate = DateTime.UtcNow
        };

        await db.Vacancies.AddAsync(vacancy);
        await db.SaveChangesAsync();
        
        var resultDto = new VacancyDto(
            vacancy.Id,
            vacancy.Title,
            vacancy.Description,
            vacancy.MinSalary,
            vacancy.MaxSalary,
            vacancy.Commission,
            userId,
            currentUser.Name,
            vacancy.Qualifications
                .Select(q => q.Name),
            vacancy.CreationDate);
        
        return Ok(resultDto);
    }
}