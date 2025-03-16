using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecruitmentAgency.Api.Dtos;
using RecruitmentAgency.Application;
using RecruitmentAgency.Domain;

namespace RecruitmentAgency.Api;

public class UserService(
    IRecruitmentAgencyContext context,
    IOptions<JwtSettings> jwtSettings)
    : IUserService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> AuthenticateAsync(LoginRequest request)
    {
        var user = await context.Users
                       .Include(u => ((Employee) u).Qualifications)
                       .Include(u => ((Employer) u).Activities)
                       .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber)
                   ?? throw new RecruitmentAgencyApplicationException("Invalid credentials");

        if (user.PasswordHash != request.Password) // TODO: hash
            throw new RecruitmentAgencyApplicationException("Invalid password");

        return GenerateAuthResponse(user);
    }

    private AuthResponse GenerateAuthResponse(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.MobilePhone, user.PhoneNumber)
        };
        
        var role = user switch
        {
            Admin => "Admin",
            Employee => "Employee",
            Employer => "Employer",
            _ => "User"
        };
        claims.Add(new(ClaimTypes.Role, role));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            SigningCredentials = new(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        // TODO: fix.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponse
        {
            Token = tokenHandler.WriteToken(token),
            Role = role,
            Expiration = token.ValidTo
        };
    }
}