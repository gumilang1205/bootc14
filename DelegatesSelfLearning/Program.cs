using System;
using System.Globalization;

namespace Delegates
{
    public delegate void MyDelegate(int num);


    class Program
    {

        static void Main(string[] args)
        {
            MyDelegate del1 = new MyDelegate(AddTen); // instance delegate
            del1(5);
            MyDelegate del2 = new MyDelegate(MultiplyByTwo);
            del2(10);
            NumberOperation(19, AddTen);
            NumberOperation(18, MultiplyByTwo);

            Console.WriteLine("Menambahkan Delegate");//multicase delegate
            MyDelegate d = new MyDelegate(AddTen);
            d += MultiplyByTwo;
            Console.WriteLine("multicast delegate");
            d(4);

            d -= AddTen; //AddTen tidak dijalankan karena dihapus
            Console.WriteLine("Memanggil multicast delegate setelah menghapus AddTen");
            d(5);

            Console.ReadLine();
        }
        public static void AddTen(int number)
        {
            Console.WriteLine($"menambahkan 10 : {number + 10}");
        }
        public static void MultiplyByTwo(int number)
        {
            Console.WriteLine($"Mengalikan dengan 2: {number * 2}");
        }
        public static void NumberOperation(int value, MyDelegate operation) //plug in methods
        {
            Console.WriteLine($"mengoperasikan pada number {value}");
            operation(value);
            Console.WriteLine("operasi selesai");
        }

    }
}