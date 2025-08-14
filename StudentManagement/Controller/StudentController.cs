using Microsoft.AspNetCore.Mvc;
using StudentManagement.Service;
using StudentManagement.DTOs;
namespace StudentManagement.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            if (students == null || !students.Any())
            {
                return NotFound("No students found.");
            }
            return Ok(students);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok(student);
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
            {
                return BadRequest("Invalid student data.");
            }
            var result = await _studentService.CreateStudentAsync(studentDto);
            if (!result)
            {
                return StatusCode(500, "An error occurred while creating the student.");
            }
            return CreatedAtAction(nameof(GetStudentById), new { id = studentDto.Id }, studentDto);
        }
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto studentDto)
        {
            if (studentDto == null || studentDto.Id != id)
            {
                return BadRequest("Invalid student data.");
            }
            var result = await _studentService.UpdateStudentAsync(id, studentDto);
            if (!result)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok("Successfully update the student.");
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return Ok("Successfully deleted the student.");
        }

    }
}