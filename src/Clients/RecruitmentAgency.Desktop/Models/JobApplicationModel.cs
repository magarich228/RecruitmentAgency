using System;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.Models;

public class JobApplicationModel
{
    public JobApplicationDto Application { get; set; }

    public string Status => Application.Status switch
    {
        ApplicationStatus._0 => "Pending",
        ApplicationStatus._1 => "Accepted",
        ApplicationStatus._2 => "Rejected",
        _ => throw new IndexOutOfRangeException()
    };
}

public class EmployerJobApplicationModel
{
    public VacancyApplicationDto Application { get; set; }

    public string Status => Application.Status switch
    {
        ApplicationStatus._0 => "Pending",
        ApplicationStatus._1 => "Accepted",
        ApplicationStatus._2 => "Rejected",
        _ => throw new IndexOutOfRangeException()
    };
    
    public bool OfferNotExists => Application.Status == ApplicationStatus._0;
}