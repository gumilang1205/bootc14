using System.Dynamic;
using System.Reflection.Metadata.Ecma335;

public class Kotak
{
    private int panjang;
    private int lebar;
    public Kotak(int panjang, int lebar)
    {
        this.panjang = panjang;
        this.lebar = lebar;
    }

    public int Panjang
    {
        get { return Panjang; }
        set
        {
            if (value > 0)
                panjang = value;
            else
                Console.WriteLine("nilai harus positif");
        }
    }
    public int Lebar
    {
        get { return Lebar; }
        set
        {
            if (value > 0)
                lebar = value;
            else
                Console.WriteLine("nilai harus positif");
        }
    }
    //public int Lebar{ get; set; }
    public int Luas()
    {
        return panjang * lebar;
    }
}