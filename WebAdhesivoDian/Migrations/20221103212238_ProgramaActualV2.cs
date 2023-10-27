using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdhesivoDian.Migrations
{
    public partial class ProgramaActualV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_ROL",
                table: "USUARIOS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ESTADO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_ID_ROL",
                table: "USUARIOS",
                column: "ID_ROL");

            migrationBuilder.AddForeignKey(
                name: "FK_USUARIOS_ROLES_ID_ROL",
                table: "USUARIOS",
                column: "ID_ROL",
                principalTable: "ROLES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_USUARIOS_ROLES_ID_ROL",
                table: "USUARIOS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropIndex(
                name: "IX_USUARIOS_ID_ROL",
                table: "USUARIOS");

            migrationBuilder.DropColumn(
                name: "ID_ROL",
                table: "USUARIOS");
        }
    }
}
