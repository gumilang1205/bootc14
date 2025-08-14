using WebAPIFaculty.DTOs;
namespace WebAPIFaculty.Service;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto> GetStudentByIdAsync(int id);
    Task<bool> CreateStudentAsync(StudentDto studentDto);
    Task<bool> UpdateStudentAsync(int id, StudentDto studentDto);
    Task<bool> DeleteStudentAsync(int id);
}