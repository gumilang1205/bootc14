using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWT.Models
{
    public class Role : IdentityRole
    {
        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}