namespace RecruitmentAgency.Application;

public class RecruitmentAgencyApplicationException(string? message = null, Exception? inner = null) : 
    Exception(message, inner);