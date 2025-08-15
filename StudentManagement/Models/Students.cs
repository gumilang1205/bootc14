using Microsoft.AspNetCore.Identity;

namespace StudentManagement.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string? StudentNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}