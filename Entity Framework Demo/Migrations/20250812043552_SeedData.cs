using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity_Framework_Demo.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Budget", "Description", "ManagerId", "Name" },
                values: new object[,]
                {
                    { 1, 500000m, "Software development and technical infrastructure", null, "Engineering" },
                    { 2, 300000m, "Revenue generation and client acquisition", null, "Sales" },
                    { 3, 200000m, "Brand promotion and market analysis", null, "Marketing" },
                    { 4, 150000m, "Employee management and company culture", null, "Human Resources" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Budget", "Description", "EndDate", "Name", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 50000m, "Complete overhaul of company website", null, "Website Redesign", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 2, 80000m, "Native mobile application for iOS and Android", null, "Mobile App Development", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 3, 35000m, "CRM integration and sales process automation", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sales Automation", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "Email", "HireDate", "IsActive", "Name", "Position", "Salary" },
                values: new object[,]
                {
                    { 1, 1, "john.doe@company.com", new DateTime(2022, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "John Doe", "Senior Developer", 75000m },
                    { 2, 1, "jane.smith@company.com", new DateTime(2022, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Jane Smith", "Frontend Developer", 68000m },
                    { 3, 2, "mike.johnson@company.com", new DateTime(2021, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mike Johnson", "Sales Manager", 82000m },
                    { 4, 3, "sarah.wilson@company.com", new DateTime(2022, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Sarah Wilson", "Marketing Specialist", 71000m },
                    { 5, 4, "david.brown@company.com", new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "David Brown", "HR Manager", 79000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
