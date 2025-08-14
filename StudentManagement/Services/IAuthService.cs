using StudentManagement.DTOs;
//using StudentManagement.Responses;

namespace StudentManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerRequest);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
    //     Task<ApiResponseDto<UserDto>> GetUserProfileAsync(string userId);
    //     Task<ApiResponseDto<List<UserDto>>> GetAllUsersAsync();
    //     Task<ApiResponseDto<bool>> AssignRoleAsync(string userId, string role);
     }
}