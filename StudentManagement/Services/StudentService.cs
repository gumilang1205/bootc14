using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Repositories;
using AutoMapper;
namespace StudentManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository, ApplicationDbContext context, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _context = context;
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

        public async Task<bool> CreateStudentAsync(StudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);

            if (student == null) return false;
            await _studentRepository.AddStudentAsync(student);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDto studentDto)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return false;
            _mapper.Map(studentDto, student);
            await _studentRepository.UpdateStudentAsync(student);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return false;
            await _studentRepository.DeleteStudentAsync(id);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}