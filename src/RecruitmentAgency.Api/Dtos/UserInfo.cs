namespace RecruitmentAgency.Api.Dtos;

public class UserInfo
{
    public string? Id { get; set; }
    public string? PhoneNumber { get; set; }
    
    // Employeer

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? MainAddress { get; set; }

    // Employee

    public string? FullName { get; set; }
    public string? Resume { get; set; }
}