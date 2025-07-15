/*int i = 1;
float f = i;
Console.WriteLine(i);
int i2 = (int)f;
 Console.WriteLine(i2);*/
/*int a = 2 / 3;
int b = 0;
int c = 5 / b;
//int d = 5 / 0;
Console.WriteLine(c);*/

/*int a = int.MaxValue;
a++;
Console.WriteLine(a == int.MinValue);*/
using System;

class OverFlowCheck
{
    static void Main(String[] args)
    {
        int a = 1000000;
        int b = 1000000;
        int x = 1000000;
        int y = 1000000;
        double o = 1000000;
        double p = 1000000;
        
        try
        {
            double q = checked(o * p);
            int z = unchecked(a * b);
            Console.WriteLine($"Hasil perkalian (checked) o*p adalah {q}");
            Console.WriteLine($"hasil perkalian (unchecked) : {z}");
            int c = checked(a * b);
            Console.WriteLine($"hasil perkalian (checked) : {c}");
        }
        catch (OverflowException e)
        {
            Console.WriteLine($"Terjadi luapan {e.Message}");
        }
    }
}





