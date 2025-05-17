using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoVacancy.Migrations
{
    /// <inheritdoc />
    public partial class fixCarritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoLinea_Carritos_idCarrito",
                table: "CarritoLinea");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritoLinea_Productos_idProducto",
                table: "CarritoLinea");

            migrationBuilder.DropForeignKey(
                name: "FK_Carritos_Usuarios_idUsuario",
                table: "Carritos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Carritos_idCarrito",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carritos",
                table: "Carritos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarritoLinea",
                table: "CarritoLinea");

            migrationBuilder.RenameTable(
                name: "Carritos",
                newName: "CarritosCabecera");

            migrationBuilder.RenameTable(
                name: "CarritoLinea",
                newName: "CarritosLineas");

            migrationBuilder.RenameIndex(
                name: "IX_Carritos_idUsuario",
                table: "CarritosCabecera",
                newName: "IX_CarritosCabecera_idUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_CarritoLinea_idProducto",
                table: "CarritosLineas",
                newName: "IX_CarritosLineas_idProducto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarritosCabecera",
                table: "CarritosCabecera",
                column: "idCarrito");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarritosLineas",
                table: "CarritosLineas",
                columns: new[] { "idCarrito", "idProducto" });

            migrationBuilder.AddForeignKey(
                name: "FK_CarritosCabecera_Usuarios_idUsuario",
                table: "CarritosCabecera",
                column: "idUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritosLineas_CarritosCabecera_idCarrito",
                table: "CarritosLineas",
                column: "idCarrito",
                principalTable: "CarritosCabecera",
                principalColumn: "idCarrito",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritosLineas_Productos_idProducto",
                table: "CarritosLineas",
                column: "idProducto",
                principalTable: "Productos",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_CarritosCabecera_idCarrito",
                table: "Pedidos",
                column: "idCarrito",
                principalTable: "CarritosCabecera",
                principalColumn: "idCarrito",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritosCabecera_Usuarios_idUsuario",
                table: "CarritosCabecera");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritosLineas_CarritosCabecera_idCarrito",
                table: "CarritosLineas");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritosLineas_Productos_idProducto",
                table: "CarritosLineas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_CarritosCabecera_idCarrito",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarritosLineas",
                table: "CarritosLineas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarritosCabecera",
                table: "CarritosCabecera");

            migrationBuilder.RenameTable(
                name: "CarritosLineas",
                newName: "CarritoLinea");

            migrationBuilder.RenameTable(
                name: "CarritosCabecera",
                newName: "Carritos");

            migrationBuilder.RenameIndex(
                name: "IX_CarritosLineas_idProducto",
                table: "CarritoLinea",
                newName: "IX_CarritoLinea_idProducto");

            migrationBuilder.RenameIndex(
                name: "IX_CarritosCabecera_idUsuario",
                table: "Carritos",
                newName: "IX_Carritos_idUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarritoLinea",
                table: "CarritoLinea",
                columns: new[] { "idCarrito", "idProducto" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carritos",
                table: "Carritos",
                column: "idCarrito");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoLinea_Carritos_idCarrito",
                table: "CarritoLinea",
                column: "idCarrito",
                principalTable: "Carritos",
                principalColumn: "idCarrito",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoLinea_Productos_idProducto",
                table: "CarritoLinea",
                column: "idProducto",
                principalTable: "Productos",
                principalColumn: "idProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carritos_Usuarios_idUsuario",
                table: "Carritos",
                column: "idUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Carritos_idCarrito",
                table: "Pedidos",
                column: "idCarrito",
                principalTable: "Carritos",
                principalColumn: "idCarrito",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
