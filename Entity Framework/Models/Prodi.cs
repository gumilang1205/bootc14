namespace EntityFramework.Models;

public class Prodi
{
    public int ProdiID { get; set; }
    public string NamaProdi { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to Fakultas
    /// This establishes a many-to-one relationship where many Prodi can belong to one Fakultas
    /// </summary>
    public int FakultasID { get; set; }
    public Fakultas Fakultas { get; set; }
    public List<Mahasiswa> Mahasiswas { get; set; }
}