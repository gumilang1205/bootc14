using Microsoft.AspNetCore.Mvc;
using StudentManagement.Service;
using StudentManagement.Dtos;
using Microsoft.AspNetCore.Authorization;
namespace StudentManagement.Controllers
{
    [Authorize]
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
            return Ok(new List<StudentDto>(students));
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdStudentDto = await _studentService.CreateStudentAsync(studentDto);
            if (createdStudentDto == null)
            {
                return StatusCode(500, "An error occurred while creating the student.");
            }
            return CreatedAtAction(nameof(GetStudentById), new { id = createdStudentDto.Id }, createdStudentDto);
        }
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (studentDto == null || studentDto.Id != id)
            {
                return BadRequest("Invalid student data.");
            }
            var result = await _studentService.UpdateStudentAsync(id, studentDto);
            if (!result)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            //return Ok("Successfully update the student.");

            return NoContent(); // 204 No Content is more appropriate for updatess
        }
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            //return Ok("Successfully deleted the student.");
            return NoContent(); // 204 No Content is more appropriate for deletions 
        }

    }
}