namespace CRUD_WebAPI.Models;
public class Mahasiswa
{
    public int MahasiswaID { get; set; }
    public string? NIM { get; set; }
    public string? NamaMahasiswa { get; set; }
    public int ProdiID { get; set; }
    public Prodi? Prodi { get; set; }
    public DateTime TanggalLahir { get; set; }
    public string? Alamat { get; set; }
    
}