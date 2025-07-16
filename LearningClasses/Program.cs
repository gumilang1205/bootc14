
using System;
namespace Classes
{
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
        void CityCar()
        {
            Console.WriteLine($"ukuran mobil city car {Ukuran} dan kecepatan maksimal {Kecepatan} km/jam");
        }
        void Informasi()
        {
            Console.WriteLine($"mobil ini adalah buatan {Merk} ukuran mobil {Ukuran} dan kecepatannya {Kecepatan} km/jam");
        }

        
     static void Main(string[] args)
        {
            Car sedan = new Car("toyota", "sedang", 80);
            sedan.CityCar();
            sedan.Informasi();

        }
    }
}