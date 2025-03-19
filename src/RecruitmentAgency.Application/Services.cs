using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Application;

public class VacancyService(IRecruitmentAgencyContext context) : BaseService(context), IVacancyService
{
    public async Task<Vacancy> CreateVacancyAsync(VacancyCreateDto dto)
    {
        var employer = await _context.Employers
                           .Include(e => e.Activities)
                           .FirstOrDefaultAsync(e => e.Id == default!)
                       ?? throw new ArgumentException("Invalid employer");

        if ((employer.Activities ?? throw new InvalidOperationException()).All(a => a.Id != dto.ActivityId))
            throw new InvalidOperationException("Employer not registered for this activity");

        var vacancy = new Vacancy
        {
            Title = dto.Title,
            Description = dto.Description,
            MinSalary = dto.MinSalary,
            MaxSalary = dto.MaxSalary,
            Commission = default,
            Employer = employer,
            Qualifications = new List<Qualification>(),
            Id = Guid.NewGuid(),
            CreationDate = DateTime.Now
        };

        // foreach (var qualificationId in dto.QualificationIds)
        // {
        //     var qualification = await _context.Qualifications.FindAsync(qualificationId)
        //                         ?? throw new ArgumentException($"Invalid qualification ID: {qualificationId}");
        //     vacancy.Qualifications.Add(qualification);
        // }

        await _context.Vacancies.AddAsync(vacancy);
        await SaveAsync();
        return vacancy;
    }

    public async Task UpdateVacancyAsync(Guid id, VacancyUpdateDto dto)
    {
        var vacancy = await VacanciesWithIncludes
                          .FirstOrDefaultAsync(v => v.Id == id)
                      ?? throw new KeyNotFoundException("Vacancy not found");

        vacancy.Title = dto.Title ?? vacancy.Title;
        vacancy.Description = dto.Description ?? vacancy.Description;
        vacancy.MinSalary = dto.MinSalary ?? vacancy.MinSalary;
        vacancy.MaxSalary = dto.MaxSalary ?? vacancy.MaxSalary;
        vacancy.Commission = dto.Commission ?? vacancy.Commission;

        if (dto.QualificationIds != null && dto.QualificationIds.Any())
        {
            var newQualifications = await _context.Qualifications
                .Where(q => dto.QualificationIds.Contains(q.Id))
                .ToListAsync();

            vacancy.Qualifications = newQualifications;
        }

        _context.Vacancies.Update(vacancy);

        await SaveAsync();
    }

    public async Task DeleteVacancyAsync(Guid id)
    {
        var vacancy = await _context.Vacancies.FindAsync(id);
        if (vacancy == null) return;

        _context.Vacancies.Remove(vacancy);
        await SaveAsync();
    }

    public async Task<VacancyDto> GetVacancyAsync(Guid id)
    {
        var vacancy = await VacanciesWithIncludes
                          .FirstOrDefaultAsync(v => v.Id == id)
                      ?? throw new KeyNotFoundException("Vacancy not found");

        return MapToDto(vacancy);
    }

    public async Task<IEnumerable<VacancyDto>> SearchVacanciesAsync(VacancySearchFilter filter)
    {
        var query = VacanciesWithIncludes.AsNoTracking();

        if (filter.MinSalary.HasValue)
            query = query.Where(v => v.MinSalary >= filter.MinSalary);

        if (filter.MaxSalary.HasValue)
            query = query.Where(v => v.MaxSalary <= filter.MaxSalary);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            query = query.Where(v => EF.Functions.Like(v.Title.ToLower(), $"%{filter.SearchTerm.ToLower()}%")
                                     || EF.Functions.Like(v.Description.ToLower(), $"%{filter.SearchTerm.ToLower()}%")
                                     || v.Qualifications!.Any(q => q.Name.ToLower() == filter.SearchTerm.ToLower()));

        var vacancies = await query
            .OrderByDescending(v => v.CreationDate)
            .ToListAsync();
        
        return (vacancies)
            .Select(MapToDto);
    }

    public async Task AddQualificationToVacancyAsync(Guid vacancyId, Guid qualificationId)
    {
        var vacancy = await _context.Vacancies
                          .Include(v => v.Qualifications)
                          .FirstOrDefaultAsync(v => v.Id == vacancyId)
                      ?? throw new KeyNotFoundException("Vacancy not found");

        var qualification = await _context.Qualifications.FindAsync(qualificationId)
                            ?? throw new KeyNotFoundException("Qualification not found");

        if ((vacancy.Qualifications ?? throw new InvalidOperationException()).All(q => q.Id != qualificationId))
        {
            vacancy.Qualifications.Add(qualification);
            await SaveAsync();
        }
    }

    private VacancyDto MapToDto(Vacancy vacancy) => new
    (
        vacancy.Id,
        vacancy.Title,
        vacancy.Description,
        vacancy.MinSalary,
        vacancy.MaxSalary,
        vacancy.Commission,
        vacancy.Employer!.Id,
        vacancy.Employer.Name,
        vacancy.Qualifications?.Select(q => q.Name) ?? Array.Empty<string>(),
        vacancy.CreationDate
    );
}

public class ReportService(IRecruitmentAgencyContext context) : BaseService(context), IReportService
{
    public async Task<CommissionReport> GenerateCommissionReportAsync(ReportFilter filter)
    {
        var query = _context.Vacancies
            .Include(v => v.Employer)
            .Include(v => v.JobOffers)
            .Where(v => v.CreationDate >= filter.StartDate && v.CreationDate <= filter.EndDate);

        if (filter.EmployerId != null)
            query = query.Where(v => v.Employer.Id == filter.EmployerId);

        var reportData = await query
            .GroupBy(v => new {v.Employer.Id, v.Employer.Name})
            .Select(g => new CommissionReportItem
            (
                g.Key.Id,
                g.Key.Name,
                g.Sum(v => v.Commission),
                g.SelectMany(v => v.JobOffers)
                    .Count(o => o.Verdict == OfferVerdict.Invitation)
            ))
            .ToListAsync();

        return new CommissionReport
        (
            filter.StartDate,
            filter.EndDate,
            reportData,
            reportData.Sum(i => i.TotalCommission)
        );
    }
}

// 1. Сервис для работы с работниками (полная реализация)
public class EmployeeService(IRecruitmentAgencyContext context) : BaseService(context), IEmployeeService
{
    public async Task<EmployeeProfileDto> CreateEmployeeAsync(EmployeeCreateDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber))
            throw new InvalidOperationException("User with this phone already exists");

        var employee = new Employee
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.Password,
            FullName = dto.FullName,
            Resume = dto.Resume
        };

        await _context.Employees.AddAsync(employee);
        await SaveAsync();
        return MapToProfileDto(employee);
    }

    public async Task UpdateEmployeeProfileAsync(string id, EmployeeUpdateDto dto)
    {
        var employee = await EmployeesWithIncludes
                           .FirstOrDefaultAsync(e => e.Id == id)
                       ?? throw new KeyNotFoundException("Employee not found");

        employee.FullName = dto.FullName ?? employee.FullName;
        employee.Resume = dto.Resume ?? employee.Resume;

        await SaveAsync();
    }

    public async Task AddQualificationToEmployeeAsync(string employeeId, Guid qualificationId)
    {
        var employee = await _context.Employees
                           .Include(e => e.Qualifications)
                           .FirstOrDefaultAsync(e => e.Id == employeeId)
                       ?? throw new KeyNotFoundException("Employee not found");

        var qualification = await _context.Qualifications.FindAsync(qualificationId)
                            ?? throw new KeyNotFoundException("Qualification not found");

        if (employee.Qualifications.Any(q => q.Id == qualificationId))
            return;

        employee.Qualifications.Add(qualification);
        await SaveAsync();
    }

    public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(EmployeeSearchFilter filter)
    {
        var query = EmployeesWithIncludes.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Profession))
        {
            query = query.Where(e => e.Qualifications
                .Any(q => q.Profession.Name == filter.Profession));
        }

        if (filter.MinExperienceYears.HasValue)
        {
            query = query.Where(e => e.Resume.Contains($"{filter.MinExperienceYears} лет"));
        }

        return await query.Select(e => MapToDto(e))
            .ToListAsync();
    }

    private EmployeeProfileDto MapToProfileDto(Employee employee) => new()
    {
        Id = employee.Id,
        FullName = employee.FullName,
        PhoneNumber = employee.PhoneNumber,
        Resume = employee.Resume,
        Qualifications = employee.Qualifications.Select(q => q.Name)
    };

    private EmployeeDto MapToDto(Employee employee) => new EmployeeDto()
    {
        Id = employee.Id,
        FullName = employee.FullName,
        PhoneNumber = employee.PhoneNumber,
        Resume = employee.Resume,
        Qualifications = employee.Qualifications.Select(q => q.Name)
    };
}

public class EmployeeDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Resume { get; set; }
    public IEnumerable<string> Qualifications { get; set; }
}

// 2. Сервис для работодателей (полная реализация)
public class EmployerService : BaseService, IEmployerService
{
    public EmployerService(IRecruitmentAgencyContext context) : base(context)
    {
    }

    public async Task<EmployerProfileDto> CreateEmployerAsync(EmployerCreateDto dto)
    {
        if (await _context.Employers.AnyAsync(e => e.Name == dto.CompanyName))
            throw new InvalidOperationException("Company already registered");

        var employer = new Employer
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.CompanyName,
            MainAddress = dto.Address,
            Description = dto.Description,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.Password
        };

        await _context.Employers.AddAsync(employer);
        await SaveAsync();
        return MapToProfileDto(employer);
    }

    public async Task UpdateCompanyInfoAsync(string id, EmployerUpdateDto dto)
    {
        var employer = await _context.Employers.FindAsync(id)
                       ?? throw new KeyNotFoundException("Employer not found");

        employer.Name = dto.CompanyName ?? employer.Name;
        employer.MainAddress = dto.Address ?? employer.MainAddress;
        employer.Description = dto.Description ?? employer.Description;

        await SaveAsync();
    }

    public async Task AddActivityToEmployerAsync(string employerId, Guid activityId)
    {
        var employer = await _context.Employers
                           .Include(e => e.Activities)
                           .FirstOrDefaultAsync(e => e.Id == employerId)
                       ?? throw new KeyNotFoundException("Employer not found");

        var activity = await _context.Activities.FindAsync(activityId)
                       ?? throw new KeyNotFoundException("Activity not found");

        if (employer.Activities.Any(a => a.Id == activityId))
            return;

        employer.Activities.Add(activity);
        await SaveAsync();
    }

    private EmployerProfileDto MapToProfileDto(Employer employer) => new()
    {
        Id = employer.Id,
        CompanyName = employer.Name,
        Address = employer.MainAddress,
        Description = employer.Description,
        Activities = employer.Activities.Select(a => a.Name),
        PhoneNumber = employer.PhoneNumber
    };
}

public class EmployerUpdateDto
{
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
}

// 3. Сервис управления заявками (полная реализация)
public class JobApplicationService : BaseService, IJobApplicationService
{
    public JobApplicationService(IRecruitmentAgencyContext context) : base(context)
    {
    }

    public async Task<JobApplication> CreateApplicationAsync(string employeeId, Guid vacancyId)
    {
        var vacancy = await _context.Vacancies
                          .Include(v => v.Qualifications)
                          .FirstOrDefaultAsync(v => v.Id == vacancyId)
                      ?? throw new KeyNotFoundException("Vacancy not found");

        var employee = await _context.Employees
                           .Include(e => e.Qualifications)
                           .FirstOrDefaultAsync(e => e.Id == employeeId)
                       ?? throw new KeyNotFoundException("Employee not found");

        // var missingQualifications = vacancy.Qualifications
        //     .Where(q => !employee.Qualifications.Contains(q))
        //     .ToList();
        //
        // if (missingQualifications.Any())
        // {
        //     throw new InvalidOperationException(
        //         $"Missing required qualifications: {string.Join(", ", missingQualifications.Select(q => q.Name))}");
        // }

        if (await _context.JobApplications
            .Include(a => a.Employee)
            .AnyAsync(a => a.Employee!.Id == employeeId && a.VacancyId == vacancyId))
        {
            throw new InvalidOperationException("Отклик на данную вакансию уже создан.");
        }

        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            VacancyId = vacancyId,
            Employee = employee,
            Vacancy = vacancy,
            ApplicationDate = DateTime.UtcNow
        };

        await _context.JobApplications.AddAsync(application);
        await SaveAsync();
        return application;
    }

    public async Task ProcessApplicationResponseAsync(Guid applicationId, bool isAccepted)
    {
        var application = await _context.JobApplications
                              .Include(a => a.Vacancy)
                              .Include(a => a.JobOffer)
                              .FirstOrDefaultAsync(a => a.Id == applicationId)
                          ?? throw new KeyNotFoundException("Application not found");

        application.JobOffer.Verdict = isAccepted ? OfferVerdict.Invitation : OfferVerdict.Rejection;

        await SaveAsync();
    }

    public async Task<JobOffer> CreateDirectOfferAsync(Guid vacancyId, string employeeId, string message,
        OfferVerdict verdict)
    {
        var vacancy = await _context.Vacancies.FindAsync(vacancyId)
                      ?? throw new KeyNotFoundException("Vacancy not found");

        var employee = await _context.Employees.FindAsync(employeeId)
                       ?? throw new KeyNotFoundException("Employee not found");

        if (await _context.JobOffers
                .Include(o => o.Employee)
                .AnyAsync(o => o.VacancyId == vacancyId &&
                               o.Employee!.Id == employeeId))
        {
            throw new InvalidOperationException("Отклик уже создан!");
        }
        
        var offer = new JobOffer
        {
            Vacancy = vacancy,
            Employee = employee,
            Message = message,
            Verdict = verdict,
            CreatedDate = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            VacancyId = vacancyId
        };

        await _context.JobOffers.AddAsync(offer);
        await SaveAsync();
        return offer;
    }
}

public class QualificationService : BaseService, IQualificationService
{
    public QualificationService(IRecruitmentAgencyContext context) : base(context)
    {
    }

    public async Task<Qualification> CreateQualificationAsync(QualificationCreateDto dto)
    {
        var profession = await _context.Professions.FindAsync(dto.ProfessionId)
                         ?? throw new ArgumentException("Invalid profession");

        if (await _context.Qualifications.AnyAsync(q => q.Name == dto.Name))
            throw new InvalidOperationException("Qualification already exists");

        var qualification = new Qualification
        {
            Name = dto.Name,
            Profession = profession,
            Id = Guid.NewGuid(),
            ProfessionId = dto.ProfessionId
        };

        await _context.Qualifications.AddAsync(qualification);
        await SaveAsync();
        return qualification;
    }

    public async Task<IEnumerable<QualificationDto>> GetQualificationsForProfessionAsync(Guid professionId)
    {
        return await _context.Qualifications
            .Include(q => q.Profession)
            .Where(q => q!.Profession!.Id == professionId)
            .Select(q => new QualificationDto(q.Id, q.Name, q.Profession!.Name))
            .ToListAsync();
    }

    public async Task LinkQualificationToProfessionAsync(Guid qualificationId, Guid professionId)
    {
        var qualification = await _context.Qualifications
                                .Include(q => q.Profession)
                                .FirstOrDefaultAsync(q => q.Id == qualificationId)
                            ?? throw new KeyNotFoundException("Qualification not found");

        var profession = await _context.Professions.FindAsync(professionId)
                         ?? throw new KeyNotFoundException("Profession not found");

        qualification.Profession = profession;
        await SaveAsync();
    }
}

public class ActivityService : BaseService, IActivityService
{
    public ActivityService(IRecruitmentAgencyContext context) : base(context)
    {
    }


    public async Task<Activity> CreateActivityAsync(string name)
    {
        if (await _context.Activities.AnyAsync(a => a.Name == name))
            throw new InvalidOperationException("Activity already exists");

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _context.Activities.AddAsync(activity);
        await SaveAsync();
        return activity;
    }

    public async Task<IEnumerable<ActivityDto>> SearchActivitiesAsync(ActivitySearchFilter filter)
    {
        var query = _context.Activities
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            query = query.Where(a => a.Name.Contains(filter.SearchTerm));

        return await query
            .Select(a => new ActivityDto(a.Id, a.Name))
            .ToListAsync();
    }

    public async Task LinkActivityToEmployerAsync(Guid activityId, string employerId)
    {
        var employer = await _context.Employers
                           .Include(e => e.Activities)
                           .FirstOrDefaultAsync(e => e.Id == employerId)
                       ?? throw new KeyNotFoundException("Employer not found");

        var activity = await _context.Activities.FindAsync(activityId)
                       ?? throw new KeyNotFoundException("Activity not found");

        if (employer.Activities!.All(a => a.Id != activityId))
        {
            employer.Activities!.Add(activity);
            await SaveAsync();
        }
    }
}

public class ProfessionService(IRecruitmentAgencyContext context) : BaseService(context), IProfessionService
{
    public async Task<Profession> CreateProfessionAsync(string name)
    {
        if (await _context.Professions.AnyAsync(p => p.Name == name))
            throw new InvalidOperationException("Profession already exists");

        var profession = new Profession
        {
            Name = name,
            Id = Guid.NewGuid()
        };
        await _context.Professions.AddAsync(profession);
        await SaveAsync();
        return profession;
    }

    public async Task<IEnumerable<ProfessionDto>> GetAllProfessionsAsync()
    {
        return await _context.Professions
            .Include(p => p.AvailableQualifications)
            .Select(p => new ProfessionDto(
                p.Id,
                p.Name,
                p.AvailableQualifications!.Select(q => q.Name)))
            .ToListAsync();
    }

    public async Task UpdateProfessionQualificationsAsync(Guid professionId, List<Guid> qualificationIds)
    {
        var profession = await _context.Professions
                             .Include(p => p.AvailableQualifications)
                             .FirstOrDefaultAsync(p => p.Id == professionId)
                         ?? throw new KeyNotFoundException("Profession not found");

        var qualifications = await _context.Qualifications
            .Where(q => qualificationIds.Contains(q.Id))
            .ToListAsync();

        profession.AvailableQualifications = qualifications;

        await SaveAsync();
    }
}

// 5. Вспомогательные DTO и модели
public record EmployeeCreateDto(
    string PhoneNumber,
    string Password,
    string FullName,
    string Resume);

public record EmployeeUpdateDto(
    string? FullName,
    string? Resume);

public record EmployerCreateDto(
    string CompanyName,
    string Address,
    string Description,
    string PhoneNumber,
    string Password);

public record QualificationCreateDto(
    string Name,
    Guid ProfessionId);

public enum ApplicationStatus
{
    Pending,
    Accepted,
    Rejected
}

public class EmployeeSearchFilter
{
    public string? Profession { get; set; }
    public int? MinExperienceYears { get; set; }
}

public record VacancyCreateDto(
    string Title,
    string Description,
    decimal MinSalary,
    decimal MaxSalary,
    Guid ActivityId);

public record VacancyUpdateDto(
    string? Title,
    string? Description,
    decimal? MinSalary,
    decimal? MaxSalary,
    decimal? Commission,
    List<Guid>? QualificationIds);

public record VacancyDto(
    Guid Id,
    string Title,
    string Description,
    decimal? MinSalary,
    decimal? MaxSalary,
    decimal Commission,
    string EmployerId,
    string EmployerName,
    IEnumerable<string> Qualifications,
    DateTime CreatedAt);

public record VacancySearchFilter(
    decimal? MinSalary,
    decimal? MaxSalary,
    string? SearchTerm);

public record CommissionReport(
    DateTime StartDate,
    DateTime EndDate,
    List<CommissionReportItem> Items,
    decimal TotalCommission);

public record CommissionReportItem(
    string EmployerId,
    string EmployerName,
    decimal TotalCommission,
    int SuccessfulOffers);

public record ActivityStatsReport(
    string ActivityName,
    int TotalVacancies,
    int TotalApplications,
    double SuccessRate);

public record ReportFilter(
    DateTime StartDate,
    DateTime EndDate,
    string? EmployerId = null);

public record QualificationDto(
    Guid Id,
    string Name,
    string ProfessionName);

public class EmployeeProfileDto
{
    public required string Id { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }
    public string? Resume { get; init; }
    public IEnumerable<string>? Qualifications { get; init; }
}

public class EmployerProfileDto
{
    public required string Id { get; init; }
    public required string CompanyName { get; init; }
    public required string Address { get; init; }
    public required string Description { get; init; }
    public required string PhoneNumber { get; init; }
    public IEnumerable<string>? Vacancies { get; init; }
    public IEnumerable<string>? Activities { get; init; }
}

public record ActivityDto(
    Guid Id,
    string Name);

public record ProfessionDto(
    Guid Id,
    string Name,
    IEnumerable<string> Qualifications);

public record JobApplicationDto(
    Guid Id,
    string EmployeeName,
    string EmployerName,
    string VacancyTitle,
    DateTime ApplicationDate,
    ApplicationStatus Status);

public record JobOfferDto(
    Guid Id,
    string VacancyTitle,
    string EmployeeName,
    OfferVerdict Verdict,
    DateTime CreatedDate,
    DateTime? ClosedDate);

// 2. Фильтры для поиска
public record EmployerSearchFilter(
    string? Name,
    Guid? ActivityId,
    string? Address);

public record ActivitySearchFilter(
    string? SearchTerm);