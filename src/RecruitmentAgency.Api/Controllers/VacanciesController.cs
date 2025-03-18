using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentAgency.Application;

namespace RecruitmentAgency.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanciesController(
    IVacancyService vacancyService,
    IJobApplicationService jobApplicationService) : ControllerBase
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
}