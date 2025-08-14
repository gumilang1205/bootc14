// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using StudentManagement.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.IdentityModel.Tokens;

// namespace StudentManagement.Services;

// public class JwtTokenService : IJwtTokenService
// {
//     private readonly IConfiguration _configuration;
//     private readonly UserManager<Student> _userManager;

//     public JwtTokenService(IConfiguration configuration, UserManager<Student> userManager)
//     {
//         _configuration = configuration;
//         _userManager = userManager;
//     }

//     public string GenerateToken(Student user, List<string> roles)
//     {
//         try
//         {
//             // Claims are like personal details on your ID card
//             // They tell us who you are and what you're allowed to do
//             // Now using Identity's built-in properties
//             var claims = new List<Claim>
//             {
//                 new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Subject (User ID from Identity)
//                 new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""), // Email from Identity
//                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
//                 new Claim(ClaimTypes.Name, user.UserName ?? ""), // Username from Identity
//                 new Claim(ClaimTypes.NameIdentifier, user.Id), // Identity user ID
//                 new Claim("UserName", user.UserName ?? "Default User")
//             };

//             // Add role claims - these define what the user can do
//             // Now using Identity's role system
//             foreach (var role in roles)
//             {
//                 claims.Add(new Claim(ClaimTypes.Role, role));
//             }

//             // Get the secret key from configuration
//             var secretKey = _configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
//             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//             var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//             // Token expiration time
//             var expirationTime = DateTime.UtcNow.AddMinutes(
//                 int.Parse(_configuration["JWT:ExpirationInMinutes"] ?? "60"));

//             // Create the actual token
//             var token = new JwtSecurityToken(
//                 issuer: _configuration["JWT:Issuer"],
//                 audience: _configuration["JWT:Audience"],
//                 claims: claims,
//                 expires: expirationTime,
//                 signingCredentials: credentials
//             );

//             // _logger.LogInformation("JWT token generated successfully for user {Email}", user.Email);
//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }
//         catch (Exception)
//         {
//             // _logger.LogError(ex, "Error generating JWT token for user {Email}", user.Email);
//             throw;
//         }
//     }

//     /// <summary>
//     /// Validate a JWT token and extract user information
//     /// This is like checking if an ID card is genuine and not expired
//     /// </summary>
//     public ClaimsPrincipal? ValidateToken(string token)
//     {
//         try
//         {
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var secretKey = _configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
//             var key = Encoding.UTF8.GetBytes(secretKey);

//             var validationParameters = new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(key),
//                 ValidateIssuer = true,
//                 ValidIssuer = _configuration["JWT:Issuer"],
//                 ValidateAudience = true,
//                 ValidAudience = _configuration["JWT:Audience"],
//                 ValidateLifetime = true,
//                 ClockSkew = TimeSpan.Zero // Remove default 5-minute tolerance
//             };

//             var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
//             return principal;
//         }
//         catch (Exception)
//         {
//             // _logger.LogWarning(ex, "Token validation failed");
//             return null;
//         }
//     }
// }