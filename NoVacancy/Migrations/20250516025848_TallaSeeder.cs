using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NoVacancy.Migrations
{
    /// <inheritdoc />
    public partial class TallaSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tallas",
                columns: new[] { "idTalla", "nombre" },
                values: new object[,]
                {
                    { 1, "XS" },
                    { 2, "S" },
                    { 3, "M" },
                    { 4, "L" },
                    { 5, "XL" },
                    { 6, "XXL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tallas",
                keyColumn: "idTalla",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tallas",
                keyColumn: "idTalla",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tallas",
                keyColumn: "idTalla",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tallas",
                keyColumn: "idTalla",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tallas",
                keyColumn: "idTalla",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tallas",
                keyColumn: "idTalla",
                keyValue: 6);
        }
    }
}
