namespace CRUD_WebAPI.Models;

public class Prodi
{
    public int ProdiID { get; set; }
    public string? NamaProdi { get; set; }
    public int FakultasID { get; set; }
    public Fakultas? Fakultas { get; set; }
    public List<Mahasiswa>? Students { get; set; }
}