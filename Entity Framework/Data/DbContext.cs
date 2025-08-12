using Entity_Framework.Migrations;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
namespace EntityFramework.Data;

public class FacultyContext : DbContext
{
    public DbSet<Fakultas> Fakultas { get; set; }
    public DbSet<Prodi> Prodi { get; set; }
    public DbSet<Mahasiswa> Mahasiswa { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=FakultasDatabase.db");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relasi: Fakultas - Prodi (1:M)
        modelBuilder.Entity<Prodi>()
            .HasOne(p => p.Fakultas)
            .WithMany(f => f.Prodis)
            .HasForeignKey(p => p.FakultasID);

        // // Relasi: Prodi - Mahasiswa (1:M)
        // modelBuilder.Entity<Mahasiswa>()
        //     .HasOne(m => m.Prodi)
        //     .WithMany(p => p.Mahasiswas)
        //     .HasForeignKey(m => m.ProdiID);

        // Data seeding
        modelBuilder.Entity<Fakultas>().HasData(
            new Fakultas { FakultasID = 1, NamaFakultas = "Fakultas Teknik" },
            new Fakultas { FakultasID = 2, NamaFakultas = "Fakultas Ilmu Komputer" },
            new Fakultas { FakultasID = 3, NamaFakultas = "Fakultas Ekonomi" },
            new Fakultas { FakultasID = 4, NamaFakultas = "Fakultas Hukum" },
            new Fakultas { FakultasID = 5, NamaFakultas = "Fakultas Kedokteran" },
            new Fakultas { FakultasID = 6, NamaFakultas = "Fakultas MIPA" },
            new Fakultas { FakultasID = 7, NamaFakultas = "Fakultas Pertanian" },
            new Fakultas { FakultasID = 8, NamaFakultas = "Fakultas Psikologi" },
            new Fakultas { FakultasID = 9, NamaFakultas = "Fakultas Sastra" },
            new Fakultas { FakultasID = 10, NamaFakultas = "Fakultas Ilmu Sosial" },
            new Fakultas { FakultasID = 11, NamaFakultas = "Fakultas Ilmu Budaya" },
            new Fakultas { FakultasID = 12, NamaFakultas = "Fakultas Kesehatan Masyarakat" },
            new Fakultas { FakultasID = 13, NamaFakultas = "Fakultas Perikanan" },
            new Fakultas { FakultasID = 14, NamaFakultas = "Fakultas Peternakan" },
            new Fakultas { FakultasID = 15, NamaFakultas = "Fakultas Teknik Sipil" },
            new Fakultas { FakultasID = 16, NamaFakultas = "Fakultas Farmasi" }
        );
        modelBuilder.Entity<Prodi>().HasData(
            new Prodi { ProdiID = 1, NamaProdi = "Teknik Elektro", FakultasID = 1 },
            new Prodi { ProdiID = 2, NamaProdi = "Sistem Informasi", FakultasID = 2 },
            new Prodi { ProdiID = 3, NamaProdi = "Manajemen", FakultasID = 3 },
            new Prodi { ProdiID = 4, NamaProdi = "Akuntansi", FakultasID = 3 },
            new Prodi { ProdiID = 5, NamaProdi = "Ilmu Hukum", FakultasID = 4 },
            new Prodi { ProdiID = 6, NamaProdi = "Kedokteran Umum", FakultasID = 5 },
            new Prodi { ProdiID = 7, NamaProdi = "Matematika", FakultasID = 6 },
            new Prodi { ProdiID = 8, NamaProdi = "Biologi", FakultasID = 6 },
            new Prodi { ProdiID = 9, NamaProdi = "Agroteknologi", FakultasID = 7 },
            new Prodi { ProdiID = 10, NamaProdi = "Psikologi", FakultasID = 8 },
            new Prodi { ProdiID = 11, NamaProdi = "Sastra Indonesia", FakultasID = 9 },
            new Prodi { ProdiID = 12, NamaProdi = "Sosiologi", FakultasID = 10 },
            new Prodi { ProdiID = 13, NamaProdi = "Ilmu Komunikasi", FakultasID = 10 },
            new Prodi { ProdiID = 14, NamaProdi = "Ilmu Budaya", FakultasID = 11 },
            new Prodi { ProdiID = 15, NamaProdi = "Kesehatan Masyarakat", FakultasID = 12 },
            new Prodi { ProdiID = 16, NamaProdi = "Teknik Sipil", FakultasID = 15 }
        );
        modelBuilder.Entity<Mahasiswa>().HasData(
            new Mahasiswa
            {
                MahasiswaID = 1,
                NamaMahasiswa = "Budi",
                NIM = "123456789",
                TanggalLahir = new DateTime(2000, 1, 1),
                ProdiID = 1
            },
            new Mahasiswa
            {
                MahasiswaID = 2,
                NamaMahasiswa = "Siti",
                NIM = "987654321",
                TanggalLahir = new DateTime(2001, 2, 2),
                ProdiID = 1
            },
            new Mahasiswa
            {
                MahasiswaID = 3,
                NamaMahasiswa = "Andi",
                NIM = "1122334455",
                TanggalLahir = new DateTime(2002, 3, 3),
                ProdiID = 2
            },
            new Mahasiswa
            {
                MahasiswaID = 4,
                NamaMahasiswa = "Dewi",
                NIM = "5566778899",
                TanggalLahir = new DateTime(2003, 4, 4),
                ProdiID = 2
            },
            new Mahasiswa
            {
                MahasiswaID = 5,
                NamaMahasiswa = "Driono",
                NIM = "123123123",
                TanggalLahir = new DateTime(2004, 5, 5),
                ProdiID = 1
            }
        );
    }

}