public class Car
{
    public string Merk;
    public string Ukuran;
    public int Kecepatan;
    public Car(string merk, string ukuran, int kecepatan)
    {
        Merk = merk;
        Ukuran = ukuran;
        Kecepatan = kecepatan;
    }
    public void CityCar()
    {
        if (Ukuran == "kecil" && Kecepatan == 80)
            Console.WriteLine($"mobil ukuran {Ukuran} adalah CityCar dan kecepatan maksimal {Kecepatan} km/jam");
        else if (Kecepatan == 100)
            Console.WriteLine($"mobil ukuran {Ukuran} adalah SUV dan kecepatan maksimal {Kecepatan} km/jam");
        else
        Console.WriteLine($"mobil ukuran {Ukuran} adalah SuperCar dan kecepatan maksimal {Kecepatan} km/jam");
    }
    public void Informasi()
    {
        Console.WriteLine($"mobil ini adalah buatan {Merk} ukuran mobil {Ukuran} dan kecepatannya {Kecepatan} km/jam");
    }
}