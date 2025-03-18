using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly UserContainer _userContainer;
    
    public RegisterViewModel(IServiceProvider serviceProvider)
    {
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();

        SelectedRole = Roles.First();
    }
    
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    
    public string[] Roles { get; } = ["employee", "employer"];
    
    // Employeer

    [ObservableProperty]
    private string? _name;
    
    [ObservableProperty]
    private string? _description;
    
    [ObservableProperty]
    private string? _mainAddress;
    
    // Employee

    [ObservableProperty]
    private string? _fullName;
    
    [ObservableProperty]
    private string? _resume;

    
    
    [ObservableProperty]
    private string _selectedRole;

    [ObservableProperty] 
    private bool _isEmployee;
    
    [ObservableProperty] 
    private bool _isEmployer;

    public async Task<AuthResponse> RegisterAsync()
    {
        _userContainer.PhoneNumber = PhoneNumber;
        _userContainer.PasswordHash = Password;
        _userContainer.ConfirmPassword = ConfirmPassword;
        _userContainer.FullName = FullName;
        _userContainer.Resume = Resume;
        _userContainer.Name = Name;
        _userContainer.Description = Description;
        _userContainer.MainAddress = MainAddress;
        
        return await _userContainer.RegisterAsync(SelectedRole);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedRole))
        {
            IsEmployee = SelectedRole == "employee";
            IsEmployer = SelectedRole == "employer";
        }
        
        base.OnPropertyChanged(e);
    }
}