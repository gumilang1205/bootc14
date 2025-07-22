string a = "abc";
string b = "Abc";
Console.WriteLine(a == b);

/*/ Membuat DateTime untuk tanggal dan waktu spesifik
DateTime tanggalLahir = new DateTime(1990, 5, 15); // 15 Mei 1990, 00:00:00 (Kind: Unspecified)
DateTime waktuRapat = new DateTime(2025, 7, 22, 14, 30, 0); // 22 Juli 2025, 14:30:00 (Kind: Unspecified)

// Waktu saat ini
DateTime sekarangLokal = DateTime.Now; // Waktu lokal sistem (Kind: Local)
DateTime sekarangUtc = DateTime.UtcNow; // Waktu UTC (Kind: Utc)

Console.WriteLine($"Tanggal Lahir: {tanggalLahir}");
Console.WriteLine($"Waktu Rapat: {waktuRapat}");
Console.WriteLine($"Sekarang Lokal: {sekarangLokal} (Kind: {sekarangLokal.Kind})");
Console.WriteLine($"Sekarang UTC: {sekarangUtc} (Kind: {sekarangUtc.Kind})");*/

/*string dateString = "2024-03-10 10:30:00";
DateTime parsedDate;
if (DateTime.TryParse(dateString, out parsedDate))
{
    Console.WriteLine($"Berhasil parse: {parsedDate}");
}
else
{
    Console.WriteLine("Gagal parse tanggal.");
}

// DateTime.ParseExact untuk format spesifik
string specificFormatDate = "12/25/2023";
DateTime christmas = DateTime.ParseExact(specificFormatDate, "MM/dd/yyyy", null);
Console.WriteLine($"Natal: {christmas.ToShortDateString()}");*/
/*DateTime utcTime = DateTime.UtcNow; // Kind: Utc
DateTime localTime = utcTime.ToLocalTime(); // Konversi ke waktu lokal sistem (Kind: Local)

Console.WriteLine($"Waktu UTC: {utcTime}");
Console.WriteLine($"Waktu Lokal dari UTC: {localTime}");

DateTime localTimeFromUnspecified = new DateTime(2025, 7, 22, 10, 0, 0, DateTimeKind.Unspecified);
// Jika Anda tahu ini sebenarnya adalah waktu lokal:
DateTime actualLocal = DateTime.SpecifyKind(localTimeFromUnspecified, DateTimeKind.Local);
Console.WriteLine($"Waktu Lokal (specified): {actualLocal}");*/

// Menggunakan waktu saat ini dengan offset lokal
/*DateTimeOffset nowWithOffset = DateTimeOffset.Now; // Otomatis menyertakan offset sistem lokal
Console.WriteLine($"Sekarang dengan Offset: {nowWithOffset} (Offset: {nowWithOffset.Offset})");

// Menggunakan waktu UTC saat ini
DateTimeOffset utcNowOffset = DateTimeOffset.UtcNow; // Offset akan 00:00:00
Console.WriteLine($"Sekarang UTC dengan Offset: {utcNowOffset} (Offset: {utcNowOffset.Offset})");

// Membuat dengan DateTime dan offset eksplisit
DateTime specificDate = new DateTime(2025, 7, 22, 10, 0, 0); // 10 AM, 22 Juli 2025
TimeSpan jakartaOffset = TimeSpan.FromHours(7); // WIB (UTC+7)
DateTimeOffset jakartaTime = new DateTimeOffset(specificDate, jakartaOffset);
Console.WriteLine($"Waktu Jakarta: {jakartaTime}");

// Membuat dari DateTime (UTC atau Lokal)
DateTimeOffset fromUtc = new DateTimeOffset(DateTime.UtcNow);
Console.WriteLine($"From UTC DateTime: {fromUtc}");*/

double d1 = 3.9;
int i1 = Convert.ToInt32(d1);    // i1 == 4 (rounds up)

double d2 = 3.2;
int i2 = Convert.ToInt32(d2);    // i2 == 3 (rounds down)

double d3 = 3.5;
int i3 = Convert.ToInt32(d3);    // i3 == 4 (banker's rounding: rounds to nearest even number)
double d4 = 8.5;
int i4 = Convert.ToInt32(d4);    // i4 == 4 (banker's rounding: rounds to nearest even number)

Console.WriteLine($"{i1} {i2} {i3} {i4}");
string s = $"It's hot this {DateTime.Now.DayOfWeek} morning";
Console.WriteLine(s);
// You can still use alignment and format strings:
Console.WriteLine($"Name={ "Mary",-20} Credit Limit={500,15:C}");
