﻿namespace RecruitmentAgency.Domain;

public class Activity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<Employer>? Employers { get; set; }
}