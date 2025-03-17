using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly UserContainer _userContainer;
    
    public LoginViewModel(IServiceProvider serviceProvider)
    {
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();
    }
    
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }

    public async Task<AuthResponse> AuthorizeAsync()
    {
        _userContainer.PhoneNumber = PhoneNumber;
        _userContainer.PasswordHash = Password;
        
        return await _userContainer.AuthorizeAsync();
    }
}