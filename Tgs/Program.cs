using System;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Masukkan angka (n): ");
        int angka = int.Parse(Console.ReadLine());
        if (angka != 0)
        {
            PrintUrutanAngkaTerbagi(angka);
        }
        else
        {
            Console.WriteLine("harus memasukan angka lebih dari 0!");
        }
    }

    public static void PrintUrutanAngkaTerbagi(int angka)
    {
        var rules = new Dictionary<int, string>();
        {
            rules.Add(3, "foo");
            rules.Add(4, "baz");
            rules.Add(5, "bar");
            rules.Add(7, "jazz");
            rules.Add(9, "huzz");
        }
        ;

        for (int i = 1; i <= angka; i++)
        {
            string output = "";
            bool divisible = false;
            foreach (var rule in rules)
            {
                if (i % rule.Key == 0)
                {
                    output += rule.Value;
                    divisible = true;
                }
            }
            if (!divisible)
            {
                Console.Write(i);
            }
            else
            {
                Console.Write(output);
            }
            if (i < angka)
            {
                Console.Write(", ");
            }
        }
        Console.WriteLine();
    }
}