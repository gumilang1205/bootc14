using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class Seeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Fakultas",
                columns: new[] { "FakultasID", "NamaFakultas" },
                values: new object[,]
                {
                    { 1, "Fakultas Teknik" },
                    { 2, "Fakultas Ilmu Komputer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Fakultas",
                keyColumn: "FakultasID",
                keyValue: 2);
        }
    }
}
