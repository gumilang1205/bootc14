using StudentManagement.Dtos;
namespace StudentManagement.Service;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto> GetStudentByIdAsync(int id);
    Task<StudentDto> CreateStudentAsync(StudentCreateDto studentDto);
    Task<bool> UpdateStudentAsync(int id, StudentDto studentDto);
    Task<bool> DeleteStudentAsync(int id);
}