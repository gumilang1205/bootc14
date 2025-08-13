using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class SeederMahasiswaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Fakultas",
                columns: new[] { "FakultasID", "NamaFakultas" },
                values: new object[,]
                {
                    { 3, "Fakultas Ekonomi" },
                    { 4, "Fakultas Hukum" },
                    { 5, "Fakultas Kedokteran" },
                    { 6, "Fakultas MIPA" },
                    { 7, "Fakultas Pertanian" },
                    { 8, "Fakultas Psikologi" },
                    { 9, "Fakultas Sastra" },
                    { 10, "Fakultas Ilmu Sosial" },
                    { 11, "Fakultas Ilmu Budaya" },
                    { 12, "Fakultas Kesehatan Masyarakat" },
                    { 13, "Fakultas Perikanan" },
                    { 14, "Fakultas Peternakan" },
                    { 15, "Fakultas Teknik Sipil" },
                    { 16, "Fakultas Farmasi" }
                });

            migrationBuilder.InsertData(
                table: "Prodi",
                columns: new[] { "ProdiID", "FakultasID", "NamaProdi" },
                values: new object[,]
                {
                    { 3, 3, "Manajemen" },
                    { 4, 3, "Akuntansi" },
                    { 5, 4, "Ilmu Hukum" },
                    { 6, 5, "Kedokteran Umum" },
                    { 7, 6, "Matematika" },
                    { 8, 6, "Biologi" },
                    { 9, 7, "Agroteknologi" },
                    { 10, 8, "Psikologi" },
                    { 11, 9, "Sastra Indonesia" },
                    { 12, 10, "Sosiologi" },
                    { 13, 10, "Ilmu Komunikasi" },
                    { 14, 11, "Ilmu Budaya" },
                    { 15, 12, "Kesehatan Masyarakat" },
                    { 16, 15, "Teknik Sipil" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 15);
        }
    }
}
