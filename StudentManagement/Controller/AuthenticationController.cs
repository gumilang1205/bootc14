using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Authentication Controller using Microsoft Identity
    /// Handles user registration, login, and profile management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Student> _userManager;
        private readonly SignInManager<Student> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<Student> userManager,
            SignInManager<Student> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenService tokenService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email is already registered." });
                }

                // Create new user
                var newUser = new Student
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    EmailConfirmed = true
                };

                // Identity handles password validation and hashing
                var result = await _userManager.CreateAsync(newUser, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Registration failed", errors });
                }

                // Assign default "User" role
                await _userManager.AddToRoleAsync(newUser, "User");

                _logger.LogInformation("User {Email} registered successfully", registerDto.Email);

                return CreatedAtAction(nameof(GetProfile), new { }, new
                {
                    message = "User registered successfully",
                    userId = newUser.Id,
                    email = newUser.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for {Email}", registerDto.Email);
                return StatusCode(500, new { message = "Registration failed due to server error" });
            }
        }

        /// <summary>
        /// Login endpoint using Microsoft Identity's SignInManager
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", loginDto.Email);
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Validate password with built-in security features
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", loginDto.Email);

                    if (result.IsLockedOut)
                        return Unauthorized(new { message = "Account is locked out." });

                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Generate JWT token
                var token = _tokenService.GenerateToken(user, roles.ToList());

                _logger.LogInformation("User {Email} logged in successfully", user.Email);

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Email = user.Email ?? "",
                    FullName = user.Name,
                    Roles = roles.ToList(),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
                return StatusCode(500, new { message = "Login failed due to server error" });
            }
        }

        /// <summary>
        /// Get current user profile information (requires authentication)
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return Unauthorized(new { message = "Invalid token." });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new UserProfileDTO
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Roles = roles.ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user profile");
                return StatusCode(500, new { message = "Failed to retrieve profile" });
            }
        }

        /// <summary>
        /// Admin-only endpoint to demonstrate role-based authorization
        /// </summary>
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            var userName = User.Identity?.Name;
            return Ok(new
            {
                message = "Welcome to the admin area!",
                user = userName,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Manager or Admin endpoint
        /// </summary>
        [HttpGet("manager-area")]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult ManagerArea()
        {
            var userName = User.Identity?.Name;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);

            return Ok(new
            {
                message = "Welcome to the manager area!",
                user = userName,
                roles = roles,
                timestamp = DateTime.UtcNow
            });
        }
    }
}