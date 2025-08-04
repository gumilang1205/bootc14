using System;

public class Program
{
    public static void Main(string[] args)
    {
        DivisibilityPrinter printer = new DivisibilityPrinter();
        printer.AddRule(3, "foo");
        printer.AddRule(4, "baz");
        printer.AddRule(5, "bar");
        printer.AddRule(7, "jazz");
        printer.AddRule(9, "huzz");
        printer.AddRule(10, "oaoe");
        Console.Write("Masukkan angka (n): ");
        int angka = int.Parse(Console.ReadLine());
        if (angka > 0)
        {
            printer.PrintUrutanAngkaTerbagi(angka);
        }
        else
        {
            Console.WriteLine("angka (n) harus lebih dari 0!");
        }
        Console.ReadLine();
    }
}
public class DivisibilityPrinter
{
    private SortedDictionary<int, string> rules;

    public DivisibilityPrinter()
    {
        rules = new SortedDictionary<int, string>();
    }
    public void AddRule(int divisor, string outputString)
    {
        if (divisor <= 0)
        {
            Console.WriteLine("Pembagi harus lebih besar dari 0!");
        }
        if (string.IsNullOrWhiteSpace(outputString))
        {
            Console.WriteLine("kata keluaran tidak boleh kosong atau spasi");
        }
        rules[divisor] = outputString;
    }
    public void PrintUrutanAngkaTerbagi(int angka)
    {
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