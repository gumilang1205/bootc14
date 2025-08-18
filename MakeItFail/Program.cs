// public class Order
// {
//     public int OrderId { get; set; }
//     public List<string> Items { get; set; }
//     public string ShippingAddress { get; set; } // Properti ini bisa null
// }

// public class BuggyOrderProcessor
// {
//     // Fungsi ini rentan terhadap bug
//     public void ProcessOrder(Order order)
//     {
//         Console.WriteLine($"Memulai pemrosesan pesanan {order.OrderId}...");

//         // BUG: Jika order.ShippingAddress adalah null, ini akan menyebabkan NullReferenceException.
//         Console.WriteLine($"Mengirimkan pesanan ke alamat: {order.ShippingAddress.ToUpper()}");

//         Console.WriteLine("Pesanan berhasil diproses.");
//     }
// }

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         Console.WriteLine("--- Versi Sebelum Debugging ---");
//         BuggyOrderProcessor processor = new BuggyOrderProcessor();

//         // Pesanan ini akan menyebabkan program crash
//         Order orderWithBug = new Order
//         {
//             OrderId = 201,
//             Items = new List<string> { "Keyboard" },
//             ShippingAddress = null // Nilai null yang menyebabkan crash
//         };

//         try
//         {
//             processor.ProcessOrder(orderWithBug);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"\nProgram mengalami crash dengan error: {ex.Message}");
//             Console.WriteLine("Bug ditemukan! Program berhenti mendadak.");
//         }
//     }
// }

// public class Order
// {
//     public int OrderId { get; set; }
//     public List<string> Items { get; set; }
//     public string ShippingAddress { get; set; }
// }

// public class FixedOrderProcessor
// {
//     // Fungsi ini telah diperbaiki
//     public void ProcessOrder(Order order)
//     {
//         Console.WriteLine($"Memulai pemrosesan pesanan {order.OrderId}...");

//         // PERBAIKAN: Melakukan pengecekan null sebelum mengakses properti
//         if (order.ShippingAddress == null)
//         {
//             // Menangani kasus di mana alamat pengiriman kosong
//             Console.WriteLine("Kesalahan: Alamat pengiriman tidak boleh kosong. Pesanan tidak dapat diproses.");
//             return; // Keluar dari fungsi secara aman
//         }

//         Console.WriteLine($"Mengirimkan pesanan ke alamat: {order.ShippingAddress.ToUpper()}");
//         Console.WriteLine("Pesanan berhasil diproses.");
//     }
// }

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         Console.WriteLine("--- Versi Sesudah Debugging 'Make It Fail' ---");
//         FixedOrderProcessor processor = new FixedOrderProcessor();

//         // --- Langkah 1: Merumuskan Hipotesis ---
//         // Hipotesis kita: Bug terjadi karena ShippingAddress bernilai null.

//         // --- Langkah 2: Membuatnya Gagal dengan Sengaja ---
//         Order orderToTest = new Order
//         {
//             OrderId = 301,
//             Items = new List<string> { "Monitor" },
//             ShippingAddress = null // Sengaja dibuat null untuk membuktikan hipotesis
//         };

//         Console.WriteLine("\n[Langkah 2: Memaksa Bug untuk Terjadi]");
//         Console.WriteLine("Mencoba memproses pesanan dengan alamat null...");

//         // Kita buktikan bahwa pengecekan null yang baru akan berfungsi
//         processor.ProcessOrder(orderToTest);

//         Console.WriteLine("\n[Hasilnya] Hipotesis terbukti. Perbaikan bekerja.");
//     }
//}


// public class Program
// {
//     public static double HitungRataRata(List<int> angka)
//     {
//         int total = 0;
//         foreach (var a in angka)
//         {
//             total += a;
//         }

//         // Bug di sini!
//         return (double)total / (angka.Count - 1);
//     }

//     public static void Main(string[] args)
//     {
//         List<int> dataAngka = new List<int> { 10, 20, 30, 40, 50 };

//         double rataRata = HitungRataRata(dataAngka);

//         Console.WriteLine($"Rata-rata dari angka-angka adalah: {rataRata}");
//     }
// }



// public class Program
// {
//     public static double HitungRataRata(List<int> angka)
//     {
//         int total = 0;
//         foreach (var a in angka)
//         {
//             total += a;
//         }
//         // Bug di sini!
//         return (double)total / (angka.Count);
//     }

//     public static void Main(string[] args)
//     {
//         List<int> dataAngka = new List<int> { 10, 20, 30, 40, 50 };

//         double rataRata = HitungRataRata(dataAngka);

//         Console.WriteLine($"Rata-rata dari angka-angka adalah: {rataRata}");
//     }
// }


// class Program
// {
//     static void Main(string[] args)
//     {
//         // Asumsi: File "data.txt" berada di lokasi yang benar
//         string filePath = "data.txt";
        
//         try
//         {
//             // Kode untuk membaca semua baris dari file
//             string[] lines = File.ReadAllLines(filePath);
            
//             Console.WriteLine("Data berhasil dibaca:");
//             foreach (string line in lines)
//             {
//                 Console.WriteLine(line);
//             }
//         }
//         catch (Exception ex)
//         {
//             // Pengembang langsung berasumsi error pada logic dan tidak fokus pada hal dasar
//             // Solusi yang mungkin dicari: "cara lain baca file csharp" atau "kenapa File.ReadAllLines error"
//             Console.WriteLine("Terjadi kesalahan saat membaca file.");
//             Console.WriteLine("Error: " + ex.Message);
//         }

//         Console.ReadKey();
//     }
// }

using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "data.txt";

        // Langkah "Check the Plug": Periksa hal paling mendasar terlebih dahulu
        // Apakah file benar-benar ada di lokasi yang diharapkan?
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Check the Plug: File tidak ditemukan!");
            Console.WriteLine($"Pastikan file '{filePath}' ada di direktori '{Directory.GetCurrentDirectory()}'.");
            return; // Berhenti di sini karena masalah dasar sudah ditemukan
        }
        
        try
        {
            // Jika pemeriksaan awal lolos, baru jalankan kode
            string[] lines = File.ReadAllLines(filePath);
            
            Console.WriteLine("Data berhasil dibaca:");
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
        catch (Exception ex)
        {
            // Catch-all exception untuk masalah yang lebih kompleks (misalnya, izin akses)
            Console.WriteLine("Terjadi kesalahan tak terduga: " + ex.Message);
        }
        
        Console.ReadKey();
    }
}