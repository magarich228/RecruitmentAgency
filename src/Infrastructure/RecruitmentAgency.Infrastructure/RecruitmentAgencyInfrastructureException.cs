using RecruitmentAgency.Application;

namespace RecruitmentAgency.Infrastructure;

public class RecruitmentAgencyInfrastructureException(string? message = null, Exception? inner = null) : 
    RecruitmentAgencyApplicationException(message, inner) { }