using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolApp.Migrations
{
    /// <inheritdoc />
    public partial class addStudentsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "StudentName" },
                values: new object[,]
                {
                    { 1, "dhaka", new DateTime(2000, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "rahat@yahoo.com", "Rahat" },
                    { 2, "dhaka", new DateTime(2001, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "rifat@yahoo.com", "Rifat" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
