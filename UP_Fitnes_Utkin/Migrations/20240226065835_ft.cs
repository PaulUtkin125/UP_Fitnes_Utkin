using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UP_Fitnes_Utkin.Migrations
{
    /// <inheritdoc />
    public partial class ft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kategorTovaras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kategorTovaras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tovar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_tovar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategID = table.Column<int>(type: "int", nullable: false),
                    Count_tekyshee = table.Column<int>(type: "int", nullable: false),
                    Price_sht = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tovar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tovar_kategorTovaras_CategID",
                        column: x => x.CategID,
                        principalTable: "kategorTovaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_users_Rol_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Rol",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tovar_CategID",
                table: "tovar",
                column: "CategID");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tovar");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "kategorTovaras");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
