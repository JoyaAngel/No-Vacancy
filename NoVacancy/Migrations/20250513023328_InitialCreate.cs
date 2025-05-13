using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoVacancy.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    constrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.idCliente);
                });

            migrationBuilder.CreateTable(
                name: "Colores",
                columns: table => new
                {
                    idColor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colores", x => x.idColor);
                });

            migrationBuilder.CreateTable(
                name: "Tallas",
                columns: table => new
                {
                    idTalla = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tallas", x => x.idTalla);
                });

            migrationBuilder.CreateTable(
                name: "Carritos",
                columns: table => new
                {
                    idCarrito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carritos", x => x.idCarrito);
                    table.ForeignKey(
                        name: "FK_Carritos_Clientes_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Clientes",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    idProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    limite = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    idTalla = table.Column<int>(type: "int", nullable: false),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    idColor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.idProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "Categorias",
                        principalColumn: "idCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Productos_Colores_idColor",
                        column: x => x.idColor,
                        principalTable: "Colores",
                        principalColumn: "idColor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Productos_Tallas_idTalla",
                        column: x => x.idTalla,
                        principalTable: "Tallas",
                        principalColumn: "idTalla",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    idPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rfc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regimen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    codigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idCarrito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.idPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Carritos_idCarrito",
                        column: x => x.idCarrito,
                        principalTable: "Carritos",
                        principalColumn: "idCarrito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarritoLinea",
                columns: table => new
                {
                    idCarrito = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarritoLinea", x => new { x.idCarrito, x.idProducto });
                    table.ForeignKey(
                        name: "FK_CarritoLinea_Carritos_idCarrito",
                        column: x => x.idCarrito,
                        principalTable: "Carritos",
                        principalColumn: "idCarrito",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarritoLinea_Productos_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Productos",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    idImagen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.idImagen);
                    table.ForeignKey(
                        name: "FK_Imagenes_Productos_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Productos",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resenias",
                columns: table => new
                {
                    idResenia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    calificacion = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resenias", x => x.idResenia);
                    table.ForeignKey(
                        name: "FK_Resenias_Productos_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Productos",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalle",
                columns: table => new
                {
                    idDetalle = table.Column<int>(type: "int", nullable: false),
                    idPedido = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle", x => new { x.idDetalle, x.idPedido });
                    table.ForeignKey(
                        name: "FK_Detalle_Pedidos_idPedido",
                        column: x => x.idPedido,
                        principalTable: "Pedidos",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarritoLinea_idProducto",
                table: "CarritoLinea",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_idCliente",
                table: "Carritos",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_correo",
                table: "Clientes",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_idPedido",
                table: "Detalle",
                column: "idPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_idProducto",
                table: "Imagenes",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_idCarrito",
                table: "Pedidos",
                column: "idCarrito");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_idCategoria",
                table: "Productos",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_idColor",
                table: "Productos",
                column: "idColor");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_idTalla",
                table: "Productos",
                column: "idTalla");

            migrationBuilder.CreateIndex(
                name: "IX_Resenias_idProducto",
                table: "Resenias",
                column: "idProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarritoLinea");

            migrationBuilder.DropTable(
                name: "Detalle");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "Resenias");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Carritos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Colores");

            migrationBuilder.DropTable(
                name: "Tallas");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
