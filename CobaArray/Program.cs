using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
namespace CobaArray
{
    public class Matrix
    {
        private int bar;
        private int kol;
        private int[,] data;
        public Matrix(int bar, int kol)
        {
            this.bar = bar;
            this.kol = kol;

            data = new int[bar, kol];
        }
        public void InputArray()
        {
            Console.WriteLine("Masukkan isi matriks ");
            for (int i = 0; i < bar; i++)
            {
                for (int j = 0; j < kol; j++)
                {
                    Console.WriteLine($"baris ke {i} kolom ke {j}");
                    data[j,i] = int.Parse(Console.ReadLine());
                }

            }

        }
        public void ViewArray()
        {
            for (int i = 0; i < bar; i++)
            {
                for (int j = 0; j < kol; j++)
                {
                    Console.Write($"|{data[i, j]}|\t");
                }
                Console.WriteLine();
            }
        }
    }
    class Program
        {
        static void Main(string[] args)
        {
            Console.Write("panjang baris : ");
            int baris = int.Parse(Console.ReadLine());
            Console.Write("panjang kolom : ");
            int kolom = int.Parse(Console.ReadLine());

            Matrix matrix1 = new Matrix(baris,kolom);
            matrix1.InputArray();

            Console.WriteLine("Hasil matrix");
            matrix1.ViewArray();
        }

        }
}