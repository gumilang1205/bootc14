using CRUD_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace EntityFramework.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Fakultas> Faculties { get; set; }
    public DbSet<Prodi> Subjects { get; set; }
    public DbSet<Mahasiswa> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=FakultasDatabase.db");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // // Relasi: Fakultas - Prodi (1:M)
        // modelBuilder.Entity<Prodi>()
        //     .HasOne(p => p.Fakultas)
        //     .WithMany(f => f.Prodis)
        //     .HasForeignKey(p => p.FakultasID);

        // // Relasi: Prodi - Mahasiswa (1:M)
        // modelBuilder.Entity<Mahasiswa>()
        //     .HasOne(m => m.Prodi)
        //     .WithMany(p => p.Students)
        //     .HasForeignKey(m => m.ProdiID);


    }

}