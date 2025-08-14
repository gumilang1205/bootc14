using Microsoft.AspNetCore.Mvc;
using WebAPIFaculty.Models;
using WebAPIFaculty.Service;
using WebAPIFaculty.DTOs;
namespace WebAPIFaculty.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // Additional methods specific to Student can be added here
    }
}