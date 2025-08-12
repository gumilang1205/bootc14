namespace EntityFramework.Models;

public class Mahasiswa
{
    public int MahasiswaID { get; set; }
    public string NamaMahasiswa { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to Prodi
    /// This establishes a many-to-one relationship where many Mahasiswa can belong to one Prodi
    /// </summary>
    public string NIM { get; set; } = string.Empty;
    public DateTime TanggalLahir { get; set; }
    public int ProdiID { get; set; }
    public Prodi Prodi { get; set; }
}