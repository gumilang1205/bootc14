using Microsoft.AspNetCore.Identity;
namespace StudentManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // Tambahkan properti peran
    }
}