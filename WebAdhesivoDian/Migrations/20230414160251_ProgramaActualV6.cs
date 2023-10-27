using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdhesivoDian.Migrations
{
    public partial class ProgramaActualV6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PASS",
                table: "USUARIOS",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NOMBRE",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EMAIL",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APELLIDOS",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Oficina",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "IdEstado",
                table: "Encabezado",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Cliente",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Cajero",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Oficina_idCliente_Codigo",
                table: "Oficina",
                columns: new[] { "idCliente", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encabezado_IdEstado",
                table: "Encabezado",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Codigo",
                table: "Cliente",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cajero_IdOficina_Codigo",
                table: "Cajero",
                columns: new[] { "IdOficina", "Codigo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Encabezado_EstadoPedido_IdEstado",
                table: "Encabezado",
                column: "IdEstado",
                principalTable: "EstadoPedido",
                principalColumn: "IdEstado",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encabezado_EstadoPedido_IdEstado",
                table: "Encabezado");

            migrationBuilder.DropIndex(
                name: "IX_Oficina_idCliente_Codigo",
                table: "Oficina");

            migrationBuilder.DropIndex(
                name: "IX_Encabezado_IdEstado",
                table: "Encabezado");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_Codigo",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cajero_IdOficina_Codigo",
                table: "Cajero");

            migrationBuilder.DropColumn(
                name: "IdEstado",
                table: "Encabezado");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PASS",
                table: "USUARIOS",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NOMBRE",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EMAIL",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "APELLIDOS",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Oficina",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Cajero",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
