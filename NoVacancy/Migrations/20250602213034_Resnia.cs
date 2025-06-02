using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoVacancy.Migrations
{
    /// <inheritdoc />
    public partial class Resnia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idPedido",
                table: "Resenias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resenias_idPedido",
                table: "Resenias",
                column: "idPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Resenias_Pedidos_idPedido",
                table: "Resenias",
                column: "idPedido",
                principalTable: "Pedidos",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resenias_Pedidos_idPedido",
                table: "Resenias");

            migrationBuilder.DropIndex(
                name: "IX_Resenias_idPedido",
                table: "Resenias");

            migrationBuilder.DropColumn(
                name: "idPedido",
                table: "Resenias");
        }
    }
}
