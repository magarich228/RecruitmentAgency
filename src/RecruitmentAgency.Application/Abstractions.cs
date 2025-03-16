using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Application;

// Сервис для работы с вакансиями
public interface IVacancyService
{
    Task<Vacancy> CreateVacancyAsync(VacancyCreateDto dto);
    Task UpdateVacancyAsync(Guid id, VacancyUpdateDto dto);
    Task DeleteVacancyAsync(Guid id);
    Task<VacancyDto> GetVacancyAsync(Guid id);
    Task<IEnumerable<VacancyDto>> SearchVacanciesAsync(VacancySearchFilter filter);
    Task AddQualificationToVacancyAsync(Guid vacancyId, Guid qualificationId);
}

// Сервис для работы с работниками
public interface IEmployeeService
{
    Task<EmployeeProfileDto> CreateEmployeeAsync(EmployeeCreateDto dto);
    Task UpdateEmployeeProfileAsync(string id, EmployeeUpdateDto dto);
    Task AddQualificationToEmployeeAsync(string employeeId, Guid qualificationId);
    Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(EmployeeSearchFilter filter);
}

// Сервис для работы с работодателями
public interface IEmployerService
{
    Task<EmployerProfileDto> CreateEmployerAsync(EmployerCreateDto dto);
    Task UpdateCompanyInfoAsync(string id, EmployerUpdateDto dto);
    Task AddActivityToEmployerAsync(string employerId, Guid activityId);
}

// Сервис для управления откликами и приглашениями
public interface IJobApplicationService
{
    Task<JobApplication> CreateApplicationAsync(string employeeId, Guid vacancyId);
    Task ProcessApplicationResponseAsync(Guid applicationId, bool isAccepted);
    Task<JobOffer> CreateDirectOfferAsync(Guid vacancyId, string employeeId, string message, OfferVerdict verdict);
}

// Сервис для работы с квалификациями и профессиями
public interface IQualificationService
{
    Task<Qualification> CreateQualificationAsync(QualificationCreateDto dto);
    Task<IEnumerable<QualificationDto>> GetQualificationsForProfessionAsync(Guid professionId);
    Task LinkQualificationToProfessionAsync(Guid qualificationId, Guid professionId);
}

// Сервис отчетности
public interface IReportService
{
    Task<CommissionReport> GenerateCommissionReportAsync(ReportFilter filter);
}

// Базовый сервис с общей логикой
public abstract class BaseService
{
    protected readonly IRecruitmentAgencyContext _context;

    protected BaseService(IRecruitmentAgencyContext context)
    {
        _context = context;
    }

    protected async Task SaveAsync() => await _context.SaveChangesAsync();
    
    protected IQueryable<Vacancy> VacanciesWithIncludes => _context.Vacancies
        .Include(v => v.Qualifications)
        .Include(v => v.Employer);

    protected IQueryable<Employee> EmployeesWithIncludes => _context.Employees
        .Include(e => e.Qualifications)
        .Include(e => e.JobApplications);
}

public interface IActivityService
{
    Task<Activity> CreateActivityAsync(string name);
    Task<IEnumerable<ActivityDto>> SearchActivitiesAsync(ActivitySearchFilter filter);
    Task LinkActivityToEmployerAsync(Guid activityId, string employerId);
}

public interface IProfessionService
{
    Task<Profession> CreateProfessionAsync(string name);
    Task<IEnumerable<ProfessionDto>> GetAllProfessionsAsync();
    Task UpdateProfessionQualificationsAsync(Guid professionId, List<Guid> qualificationIds);
}