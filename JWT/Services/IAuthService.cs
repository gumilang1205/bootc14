using JWT.Dtos;

namespace JWT.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<ApiResponseDto<UserDto>> GetUserProfileAsync(string userId);
        Task<ApiResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponseDto<bool>> AssignRoleAsync(string userId, string role);
    }
}