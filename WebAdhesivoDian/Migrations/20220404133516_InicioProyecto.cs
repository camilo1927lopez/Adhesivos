using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdhesivoDian.Migrations
{
    public partial class InicioProyecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UltimoPedido = table.Column<int>(type: "int", nullable: false),
                    ConsecutivoCadena = table.Column<int>(type: "int", nullable: false),
                    FormateoCodigoFijo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Formato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaBaseSalida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Encabezado",
                columns: table => new
                {
                    IdEncabezado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    NombrePedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreArchivoCargado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encabezado", x => x.IdEncabezado);
                    table.ForeignKey(
                        name: "FK_Encabezado_Cliente_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateTable(
                name: "Oficina",
                columns: table => new
                {
                    IdOficina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCliente = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoMunucio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoCiudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDireccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destinatario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reponsable1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reponsable2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reponsable3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficina", x => x.IdOficina);
                    table.ForeignKey(
                        name: "FK_Oficina_Cliente_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateTable(
                name: "Cajero",
                columns: table => new
                {
                    IdCajero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOficina = table.Column<int>(type: "int", nullable: false),
                    ultimaNumeracion = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    OficinaIdOficina = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajero", x => x.IdCajero);
                    table.ForeignKey(
                        name: "FK_Cajero_Oficina_OficinaIdOficina",
                        column: x => x.OficinaIdOficina,
                        principalTable: "Oficina",
                        principalColumn: "IdOficina");
                });

            migrationBuilder.CreateTable(
                name: "DetalleEncabezado",
                columns: table => new
                {
                    IdDetalleEncabezado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEncabezado = table.Column<int>(type: "int", nullable: false),
                    IdCajero = table.Column<int>(type: "int", nullable: false),
                    CantidadInicial = table.Column<int>(type: "int", nullable: false),
                    CantidadFinal = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ConsectivoinicialCadena = table.Column<int>(type: "int", nullable: false),
                    ConsectivoFinalCadena = table.Column<int>(type: "int", nullable: false),
                    EncabezadoIdEncabezado = table.Column<int>(type: "int", nullable: true),
                    CajeroIdCajero = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleEncabezado", x => x.IdDetalleEncabezado);
                    table.ForeignKey(
                        name: "FK_DetalleEncabezado_Cajero_CajeroIdCajero",
                        column: x => x.CajeroIdCajero,
                        principalTable: "Cajero",
                        principalColumn: "IdCajero");
                    table.ForeignKey(
                        name: "FK_DetalleEncabezado_Encabezado_EncabezadoIdEncabezado",
                        column: x => x.EncabezadoIdEncabezado,
                        principalTable: "Encabezado",
                        principalColumn: "IdEncabezado");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cajero_OficinaIdOficina",
                table: "Cajero",
                column: "OficinaIdOficina");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEncabezado_CajeroIdCajero",
                table: "DetalleEncabezado",
                column: "CajeroIdCajero");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEncabezado_EncabezadoIdEncabezado",
                table: "DetalleEncabezado",
                column: "EncabezadoIdEncabezado");

            migrationBuilder.CreateIndex(
                name: "IX_Encabezado_ClienteIdCliente",
                table: "Encabezado",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Oficina_ClienteIdCliente",
                table: "Oficina",
                column: "ClienteIdCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleEncabezado");

            migrationBuilder.DropTable(
                name: "Cajero");

            migrationBuilder.DropTable(
                name: "Encabezado");

            migrationBuilder.DropTable(
                name: "Oficina");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
