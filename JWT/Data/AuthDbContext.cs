using JWT.Dtos;
using JWT.Models;

;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWT.Data
{
    public class AuthDbContext : IdentityDbContext<User, Role, string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Role entity
            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed default roles
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator with full access",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Regular user with limited access",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}