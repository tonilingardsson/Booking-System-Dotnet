using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booking_System_Dotnet.Migrations
{
    /// <inheritdoc />
    public partial class SeedCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmailAddress", "LastName", "PhoneNumber" },
                values: new object[] { "antonio.gonzalez@gmail.com", "Gonzalez", "123-456-7890" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 2, "maria.rodriguez@gmail.com", "Maria", "Rodriguez", "098-765-4321" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "Id", "CourtName" },
                values: new object[,]
                {
                    { 1, "Rafa Nadal" },
                    { 2, "Roger Federer" },
                    { 3, "Björn Borg" }
                });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmailAddress", "LastName", "PhoneNumber" },
                values: new object[] { "antonio@luna.com", "Luna", "0729291305" });
        }
    }
}
