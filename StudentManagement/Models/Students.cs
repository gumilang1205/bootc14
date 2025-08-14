using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Student : IdentityUser
    {
        public int StudentID { get; set; }
        public string? StudentNumber { get; set; }
        public string? Name { get; set; }
    }
}