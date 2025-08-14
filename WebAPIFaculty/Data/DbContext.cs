using WebAPIFaculty.Models;
using Microsoft.EntityFrameworkCore;
namespace WebAPIFaculty.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=DataStudent.db");
            }
        }
        // public DbSet<Faculty> Faculties { get; set; }
        // public DbSet<Course> Courses { get; set; }
        

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // modelBuilder.Entity<Faculty>().ToTable("Faculties");
        //     // modelBuilder.Entity<Course>().ToTable("Courses");
        //     modelBuilder.Entity<Student>().ToTable("Students");
        // }
    }
}