using RecruitmentAgency.Application;

namespace RecruitmentAgency.Api;

public class RecruitmentAgencyApiException(string? message = null, Exception? inner = null) : 
    RecruitmentAgencyApplicationException(message, inner) { }