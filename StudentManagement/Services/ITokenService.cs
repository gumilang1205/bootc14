using StudentManagement.Models;
//using StudentManagement.Models.Domain;
using System.Security.Claims;

namespace StudentManagement.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(Student user);
        ClaimsPrincipal? ValidateToken(string token);
        Task<string> GenerateRefreshTokenAsync();
    }
}