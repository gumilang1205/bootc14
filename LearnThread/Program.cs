using System;
using System.Diagnostics.Metrics;
using System.Diagnostics.Tracing;
using System.Threading;

class Program
{
    static int counter = 0;
    static void Main()
    {
        Thread t = new Thread(WriteY); // Buat thread baru, jalankan WriteY()
        Thread t2 = new Thread(WriteY);
        t.Start();                   // Mulai thread baru
        CekStatus(t);
        t.Join();
        t2.Start();                   // Mulai thread baru
        t2.Join();
        // Sementara itu, thread utama menjalankan tugas lain
        // for (int i = 0; i < 10000; i++) { counter++; }
        Console.WriteLine(counter);

    }
    static void CekStatus(Thread t)
    {
        Console.WriteLine("status" + t.IsAlive);
    }
    static void WriteY()
    {
        for (int i = 0; i < 1000000; i++)
        {
            counter++;
        }

    }
}