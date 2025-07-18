
using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
namespace Classes
{
    class Program
    {
        static void WriteCube()
        {
            Console.WriteLine(Cube(3));
            int Cube(int value)=> value*value*value;
        }
        static void Main(string[] args)
        {
            /* Car sedan = new Car("toyota", "kecil", 80);
             sedan.CityCar();
             sedan.Informasi();
             Console.WriteLine();

             Car k = new Car("mitsubishi", "sedang", 100);
             k.CityCar();
             k.Informasi();
             Console.WriteLine();

             Car s = new Car("Ferrari", "sedang", 500);
             s.CityCar();
             s.Informasi();

             Kotak j = new Kotak(8, 9);
             Console.WriteLine($"luas kotak {j.Luas()}");*/

            Segitiga l = new Segitiga();
            l.Alas = 6;
            l.Tinggi = 9;
            Console.WriteLine($"Luas Segitiga {l.Alas} * {l.Tinggi} = " + l.Luas());
            Console.WriteLine($"hasil dari x*2 = {l.Foo(3)}");
            l.Display(10);
            Console.WriteLine();
            WriteCube();


        }
    }

}
