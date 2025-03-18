using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Application;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JopApplicationController(
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
}