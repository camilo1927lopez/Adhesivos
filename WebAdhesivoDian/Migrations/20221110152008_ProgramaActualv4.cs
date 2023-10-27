using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdhesivoDian.Migrations
{
    public partial class ProgramaActualv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    ID_Modulos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODIGO = table.Column<int>(type: "int", nullable: false),
                    NOMBRE_MODULO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCION_MODULO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ESTADO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.ID_Modulos);
                });

            migrationBuilder.CreateTable(
                name: "PermisosRolModulo",
                columns: table => new
                {
                    ID_Permiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo_permiso = table.Column<int>(type: "int", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_Rol = table.Column<int>(type: "int", nullable: false),
                    Id_Modulo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosRolModulo", x => x.ID_Permiso);
                    table.ForeignKey(
                        name: "FK_PermisosRolModulo_Modulos_Id_Modulo",
                        column: x => x.Id_Modulo,
                        principalTable: "Modulos",
                        principalColumn: "ID_Modulos",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermisosRolModulo_ROLES_Id_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermisosRolModulo_Id_Modulo",
                table: "PermisosRolModulo",
                column: "Id_Modulo");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosRolModulo_Id_Rol",
                table: "PermisosRolModulo",
                column: "Id_Rol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermisosRolModulo");

            migrationBuilder.DropTable(
                name: "Modulos");
        }
    }
}
