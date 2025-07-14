// See https://aka.ms/new-console-template for more information
class program
{
    static void Main(String[] args)
    {
        int[] a = new int[15];
        for (int i = 1; i <=a.Length; i++)
        {
            if (i % 3 == 0 && i % 5 ==0)
                Console.Write("foobar");
            else if (i % 5 == 0)
                Console.Write("bar,");
            else if (i % 3 == 0)
                Console.Write("foo,");
            else
                Console.Write(i+",");
        }

    }
}