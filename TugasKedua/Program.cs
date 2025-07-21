class Program
{
    static int angka;

    static void Main(String[] args)
    {
        Console.Write("Masukkan Maksimal Angka : ");
        angka = int.Parse(Console.ReadLine());
        Condition();
    }
    static void Condition()
    {
        for (int i = 1; i <= angka; i++)
        {
            if (i % 7 == 0 && i % 5 == 0 && i % 3 == 0)
                Console.Write("foobarjazz,");
            else if (i % 7 == 0 && i % 5 == 0)
                Console.Write("barjazz,");
            else if (i % 7 == 0 && i % 3 == 0)
                Console.Write("foojazz,");
            else if (i % 7 == 0)
                Console.Write("jazz,");
            else if (i % 3 == 0 && i % 5 == 0)
                Console.Write("foobar,");
            else if (i % 5 == 0)
                Console.Write("bar,");
            else if (i % 3 == 0)
                Console.Write("foo,");
            else
                Console.Write(i + ",");
        }
    }
}