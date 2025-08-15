using JWT.Models;
using System.Security.Claims;

namespace JWT.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
        ClaimsPrincipal? ValidateToken(string token);
        Task<string> GenerateRefreshTokenAsync();
    }
}