using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Application;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobApplicationController(
    IRecruitmentAgencyContext db) : ControllerBase
{
    [HttpGet("list")]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> ListAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest("User not found");

        var userApplications = await db.JobApplications
            .Include(a => a.Employee)
            .Include(a => a.Vacancy)
            .ThenInclude(v => v.Employer)
            .Include(a => a.JobOffer)
            .Where(a => a.Employee!.Id == userId)
            .ToListAsync();

        var applicationDtos = userApplications.Select(a =>
        {
            var status = a.JobOffer != null
                ? (a.JobOffer.Verdict == OfferVerdict.Invitation)
                    ? ApplicationStatus.Accepted
                    : ApplicationStatus.Rejected
                : ApplicationStatus.Pending;

            return new JobApplicationDto(a.Id, a.Employee!.FullName, a.Vacancy!.Employer!.Name, a.Vacancy!.Title,
                a.ApplicationDate, status);
        });

        return Ok(applicationDtos);
    }

    [HttpGet("listForEmployer")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<IEnumerable<VacancyApplicationDto>>> ListForEmployerAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId == null)
            return BadRequest("User not found");

        var applications = await db.JobApplications
            .Include(a => a.Employee)
            .Include(a => a.Vacancy)
            .ThenInclude(v => v!.Employer)
            .Include(a => a.JobOffer)
            .Where(a => a.Vacancy!.Employer!.Id == userId)
            .ToListAsync();

        var applicationDtos = applications.Select(a =>
        {
            var status = a.JobOffer != null
                ? (a.JobOffer.Verdict == OfferVerdict.Invitation)
                    ? ApplicationStatus.Accepted
                    : ApplicationStatus.Rejected
                : ApplicationStatus.Pending;

            return new VacancyApplicationDto()
            {
                Id = a.Id,
                EmployerName = a.Vacancy!.Employer!.Name,
                EmployeeId = a.Employee!.Id,
                EmployeeName = a.Employee!.FullName,
                VacancyId = a.Vacancy!.Id,
                VacancyTitle = a.Vacancy!.Title,
                ApplicationDate = a.ApplicationDate,
                Status = status
            };
        });

        return Ok(applicationDtos);
    }

    [HttpPost("offer")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> OfferAsync([FromBody] CreateOfferDto input)
    {
        var application = await db.JobApplications
            .Include(a => a.Employee)
            .Include(a => a.JobOffer)
            .FirstOrDefaultAsync(a => a.Id == input.ApplicationId &&
                                      a.Employee!.Id == input.EmployeeId);

        if (application is null)
        {
            return NotFound("Отклик не найден");
        }

        if (application.JobOffer is not null)
        {
            return BadRequest("Ответ на отклик уже был создан");
        }

        var offer = new JobOffer()
        {
            Id = Guid.NewGuid(),
            Verdict = input.Verdict,
            Message = input.Message,
            JobApplicationId = application.Id,
            CreatedDate = DateTime.UtcNow,
            VacancyId = application.VacancyId
        };
        
        await db.JobOffers.AddAsync(offer);
        await db.SaveChangesAsync();
        
        return Ok();
    }
}

public class VacancyApplicationDto
{
    public required Guid Id { get; set; }
    public required string EmployerName { get; set; } = null!;
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; } = null!;
    public required Guid VacancyId { get; set; }
    public required string VacancyTitle { get; set; } = null!;
    public DateTime ApplicationDate { get; set; }
    public ApplicationStatus Status { get; set; }
}

public class CreateOfferDto
{
    public required Guid ApplicationId { get; set; }
    public required string EmployeeId { get; set; }
    public string? Message { get; set; }
    public required OfferVerdict Verdict { get; set; }
}