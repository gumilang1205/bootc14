using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Repositories;
using AutoMapper;
namespace StudentManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            if (students == null || !students.Any()) return new List<StudentDto>();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return null;
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateStudentAsync(StudentCreateDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            string year = DateTime.Now.Year.ToString();
            string uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            student.StudentNumber = $"ST-{year}-{uniqueId}";

            if (student == null)
            {
                return null;
            }
            var createdStudent = await _studentRepository.AddStudentAsync(student);
            return _mapper.Map<StudentDto>(createdStudent);
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDto studentDto)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return false;
            _mapper.Map(studentDto, student);
            await _studentRepository.UpdateStudentAsync(student);
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return false;
            await _studentRepository.DeleteStudentAsync(id);
            return true;
        }
    }
}