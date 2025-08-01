﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Delegatess
{
    delegate int Transformer(int x);
    delegate void SomeDelegate();
    delegate TResult MyGenericTransformer<TInput, TResult>(TInput input);
    class Program
    {
        static void SomeMethod1()
        {
            Console.WriteLine("isi Method 1");
        }
        static void SomeMethod2()
        {
            Console.WriteLine("isi Method 2");
        }
        static int Multiply(int y) => y * 2;
        int Square(int x)
        {
            return x * x;
        }
        static int Pembagian(int z)
        {
            int pembagian = z / 2;
            Console.WriteLine($"hasil dari pembagian {z}/2 adalah {pembagian}");
            return pembagian;

        }

        static int GetStringLegth(string s)
        {
            Console.Write($"Length of {s} adalah ");
            return s.Length;
        }
        static double HalfValue(double l)
        {
            Console.Write($"Setengah dari {l} adalah ");
            return l / 2.0;
        }
        static void Main(string[] args)
        {

            //static
            Transformer u = Multiply;
            int value = u(4);
            Console.WriteLine($"hasil multiply 2 adalah {value}");

            //instance
            Program p = new Program();
            Transformer t = p.Square;
            int result = t(3);
            Console.WriteLine($"hasil dari x*x adalah {result}");
            //multicase delegates

            Transformer intTransformer = Multiply;
            intTransformer += Pembagian;
            int hasilMulticast = intTransformer(9);
            Console.WriteLine($"hasil multiply adalah {hasilMulticast}");

            SomeDelegate multiCast = SomeMethod1;
            multiCast += SomeMethod2;
            //multiCast -= SomeMethod1;
            multiCast();

            //generic delegate types
            Console.WriteLine("Menggunakan MyGenericTransformer<int,int>");
            MyGenericTransformer<int, int> pembagianDelegate = Pembagian;
            int pembagianHasil = pembagianDelegate(11);
            Console.WriteLine();
            Console.WriteLine("Menggunakan MyGenericTransformer<string,int>");
            MyGenericTransformer<string, int> LengthDelegate = GetStringLegth;
            int lenghtString = LengthDelegate("ayo semangat pasti bisa");
            Console.WriteLine(lenghtString);
            Console.WriteLine();
            Console.WriteLine("Menggunakan MyGenericTransformer<double,double>");
            MyGenericTransformer<double, double> setengahDelegate = HalfValue;
            double setengahHasil = setengahDelegate(11);
            Console.WriteLine(setengahHasil);

            //Console.ReadLine();

        }
    }

}