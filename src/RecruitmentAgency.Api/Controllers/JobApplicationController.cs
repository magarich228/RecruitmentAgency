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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new InvalidOperationException("User not found");

        var userApplications = await db.JobApplications
            .Include(a => a.Employee)
            .Include(a => a.Vacancy)
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

            return new JobApplicationDto(a.Id, a.Employee!.FullName, a.Vacancy!.Title, a.ApplicationDate, status);
        });

        return Ok(applicationDtos);
    }

    [HttpGet("listForEmployer")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<IEnumerable<VacancyApplicationDto>>> ListForEmployerAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new InvalidOperationException("User not found");

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
}

public class VacancyApplicationDto
{
    public required Guid Id { get; set; }
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; } = null!;
    public required Guid VacancyId { get; set; }
    public required string VacancyTitle { get; set; } = null!;
    public DateTime ApplicationDate { get; set; }
    public ApplicationStatus Status { get; set; }
}