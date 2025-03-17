using Microsoft.AspNetCore.Mvc;
using RecruitmentAgency.Application;

namespace RecruitmentAgency.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanciesController(IVacancyService vacancyService) : ControllerBase
{
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<VacancyDto>>> SearchAsync([FromQuery] VacancySearchFilter filter)
    {
        var result = await vacancyService.SearchVacanciesAsync(filter);

        return Ok(result);
    }
}