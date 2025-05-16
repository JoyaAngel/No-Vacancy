using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoVacancy.Migrations
{
    /// <inheritdoc />
    public partial class fixColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "codigo",
                table: "Colores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo",
                table: "Colores");
        }
    }
}
