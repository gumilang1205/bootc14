namespace CRUD_WebAPI.Models;

public class Fakultas
{
    public int FakultasID { get; set; }
    public string? NamaFakultas { get; set; }
    public List<Prodi>? Prodis { get; set; }
}