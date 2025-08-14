using System.Security.Claims;
using StudentManagement.Models;

namespace StudentManagement;

public interface IJwtTokenService
{
    string GenerateToken(Student user, List<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}