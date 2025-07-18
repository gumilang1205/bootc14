public class Segitiga
{
    public Segitiga() { }
    public int Alas { get; set; }
    public int Tinggi { get; set; }
    public int Foo(int x) => x * 2;
    public void Display(int B) => Console.WriteLine(B);

    public int Luas()
    {
        return (Alas * Tinggi) / 2;
    }
}