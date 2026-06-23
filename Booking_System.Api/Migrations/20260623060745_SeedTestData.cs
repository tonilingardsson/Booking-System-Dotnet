using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking_System_Dotnet.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { 1, "antonio@luna.com", "Antonio", "Luna", "0729291305" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
