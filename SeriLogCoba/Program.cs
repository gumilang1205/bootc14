using Serilog; // Namespace penting untuk Serilog

public class Program
{
    public static void Main(string[] args)
    {
        // Konfigurasi Logger Serilog global
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console() // Arahkan log ke konsol
            .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day) // Arahkan log ke file, membuat file baru setiap hari
            .CreateLogger(); // Selesaikan konfigurasi logger

        try
        {
            Log.Information("Starting up the application..."); // Entri log awal
            CreateHostBuilder(args).Build().Run(); // Bangun dan jalankan host
        }
        catch (Exception ex)
        {
            // Log kegagalan fatal
            Log.Fatal(ex, "Application failed to start unexpectedly.");
        }
        finally
        {
            // Pastikan semua log tertulis sebelum aplikasi keluar
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog() // Integrasikan Serilog dengan host builder .NET
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}