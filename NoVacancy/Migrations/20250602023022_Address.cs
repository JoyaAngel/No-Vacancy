using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NoVacancy.Migrations
{
    /// <inheritdoc />
    public partial class Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 8);

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

            migrationBuilder.AddColumn<string>(
                name: "Calle",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostalEnvio",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colonia",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraPedido",
                table: "Pedidos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Calle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colonia",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calle",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "CodigoPostalEnvio",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Colonia",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FechaHoraPedido",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Calle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Colonia",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "idCategoria", "nombre" },
                values: new object[,]
                {
                    { 1, "Camisetas" },
                    { 2, "Pantalones" },
                    { 3, "Vestidos" },
                    { 4, "Faldas" },
                    { 5, "Chaquetas" },
                    { 6, "Ropa interior" },
                    { 7, "Zapatos" },
                    { 8, "Accesorios" }
                });

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
    }
}
