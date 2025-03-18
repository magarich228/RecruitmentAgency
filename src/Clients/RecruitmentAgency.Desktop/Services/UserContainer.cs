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

    public string? ConfirmPassword { get; set; }

    // Employeer

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? MainAddress { get; set; }

    // Employee

    public string? FullName { get; set; }
    public string? Resume { get; set; }

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

        Client.Token = response.Token;

        return (this.Auth = response);
    }

    public async Task<AuthResponse> RegisterAsync(string role)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<Client>();

        var response = (role) switch
        {
            "employer" =>
                await client.RegisterAsEmployerAsync(new RegisterEmployerRequest()
                {
                    PhoneNumber = this.PhoneNumber,
                    Password = this.PasswordHash,
                    ConfirmPassword = this.ConfirmPassword,
                    Description = this.Description,
                    MainAddress = this.MainAddress,
                    Name = this.Name
                }),
            "employee" =>
                await client.RegisterAsEmployeeAsync(new RegisterEmployeeRequest()
                {
                    PhoneNumber = this.PhoneNumber,
                    Password = this.PasswordHash,
                    ConfirmPassword = this.ConfirmPassword,
                    Resume = this.Resume,
                    FullName = this.FullName
                }),
            _ => throw new ArgumentException("Неизвестная роль")
        };

        Client.Token = response.Token;

        return (this.Auth = response);
    }

    protected virtual void OnAuthChanged(AuthResponse? e)
    {
        AuthChanged?.Invoke(this, e);
    }
}