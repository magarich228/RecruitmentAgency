using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop.Services;

public class UserContainer(IServiceProvider serviceProvider)
{
    private AuthResponse? _auth;
    
    public string? PhoneNumber { get; set; }
    public string? PasswordHash { get; set; }
    
    public AuthResponse? Auth
    {
        get => _auth;

        set
        {
            _auth = value;
            OnAuthChanged(_auth);
        }
    }
    
    public event EventHandler<AuthResponse?>? AuthChanged; 
    
    public async Task<AuthResponse> AuthorizeAsync()
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();
        
        var response = await client.LoginAsync(new LoginRequest()
        {
            PhoneNumber = this.PhoneNumber,
            Password = this.PasswordHash
        });
        
        return (this.Auth = response);
    }

    protected virtual void OnAuthChanged(AuthResponse? e)
    {
        AuthChanged?.Invoke(this, e);
    }
}