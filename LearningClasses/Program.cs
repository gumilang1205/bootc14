
using System;
namespace Classes
{
    class Program
    {
        static void Main(string[] args)
        {
            Car sedan = new Car("toyota", "kecil", 80);
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

        }
    }
    
}
