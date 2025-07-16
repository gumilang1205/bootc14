public class Segitiga
{
    public Segitiga() { }
    public int Alas { get; set; }
    public int Tinggi { get; set; }

    public int Luas()
    {
       return (Alas* Tinggi)/2;
    }
}