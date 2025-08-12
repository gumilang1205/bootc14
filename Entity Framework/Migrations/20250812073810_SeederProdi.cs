using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class SeederProdi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Prodi",
                columns: new[] { "ProdiID", "FakultasID", "NamaProdi" },
                values: new object[,]
                {
                    { 1, 1, "Teknik Elektro" },
                    { 2, 2, "Sistem Informasi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prodi",
                keyColumn: "ProdiID",
                keyValue: 2);
        }
    }
}
