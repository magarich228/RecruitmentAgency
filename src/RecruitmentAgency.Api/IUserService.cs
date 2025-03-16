using RecruitmentAgency.Api.Dtos;

namespace RecruitmentAgency.Api;

public interface IUserService
{
    Task<AuthResponse> AuthenticateAsync(LoginRequest request);
}