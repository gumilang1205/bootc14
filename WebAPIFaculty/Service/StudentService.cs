using WebAPIFaculty.Data;
using WebAPIFaculty.Models;
using WebAPIFaculty.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace WebAPIFaculty.Service
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> CreateStudentAsync(StudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            await _context.Students.AddAsync(student);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _mapper.Map(studentDto, student);
            _context.Students.Update(student);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            return await _context.SaveChangesAsync() > 0;
        }
    }
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto> GetStudentByIdAsync(int id);
        Task<bool> CreateStudentAsync(StudentDto studentDto);
        Task<bool> UpdateStudentAsync(int id, StudentDto studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}