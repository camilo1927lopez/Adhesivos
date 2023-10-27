using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdhesivoDian.Migrations
{
    public partial class ProgramaActualV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TIPO",
                table: "USUARIOS");

            migrationBuilder.AddColumn<bool>(
                name: "ESTADO",
                table: "USUARIOS",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ESTADO",
                table: "USUARIOS");

            migrationBuilder.AddColumn<string>(
                name: "TIPO",
                table: "USUARIOS",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
