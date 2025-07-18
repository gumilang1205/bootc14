using System;
using System.Globalization;

namespace Delegates
{
    public delegate void MyDelegate(int num);
    public delegate void GenericDelegate<T>(T data);
    public delegate string DataFormatter(string input);

    class Program
    {

        static void Main(string[] args)
        {
            /* MyDelegate del1 = new MyDelegate(AddTen); // instance delegate
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
             d(5);*/

            //generic
            Console.WriteLine("Generic Delegates Type");
            GenericDelegate<int> intProcessor = PrintInteger;
            intProcessor(3);
            GenericDelegate<string> stringProcessor = PrintString;
            stringProcessor("U can do it");

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
        public static void PrintInteger(int numm)
        {
            Console.WriteLine($"interger :{numm}");
        }
        public static void PrintString(string text)
        {
            Console.WriteLine($"String : {text}");
        }
        public interface IReporter
        {
            void ReportData(string data, DataFormatter formatter);
            void SetReportTittle(string title);
        }
        public class ConsoleReporter : IReporter
        {
            private string _currentTitle = "Laporan Default";
            public void ReportData(string data, DataFormatter formatter)
            {
                Console.WriteLine($"Laporan : {_currentTitle}");
                string formattedData = formatter(data);
                Console.WriteLine($"Data terformat : {formattedData}");
            }
        }

    }
}