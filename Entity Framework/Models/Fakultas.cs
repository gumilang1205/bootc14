namespace EntityFramework.Models;
public class Fakultas
{
    public int FakultasID { get; set; }
    public string NamaFakultas { get; set; } = string.Empty;
    public List<Prodi> Prodis { get; set; }
}