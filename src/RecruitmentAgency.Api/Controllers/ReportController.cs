using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Application;

namespace RecruitmentAgency.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class ReportController(
    IRecruitmentAgencyContext db) : ControllerBase
{
    [HttpGet("report")]
    public async Task<ActionResult<ReportDto>> ReportAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var commissionIncome = await db.JobOffers
            .Include(o => o.Vacancy)
            .Where(o => o.CreatedDate >= from.ToUniversalTime() && 
                        o.CreatedDate <= to.ToUniversalTime())
            .SumAsync(o => o.Vacancy!.Commission);

        var report = new ReportDto()
        {
            CommissionIncome = commissionIncome
        };

        return Ok(report);
    }
}

public class ReportDto
{
    public required decimal CommissionIncome { get; set; }
}