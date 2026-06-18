using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNPT.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MENU",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PARENT_ID = table.Column<int>(type: "INTEGER", nullable: true),
                    MENU_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    MENU_TITLE = table.Column<string>(type: "TEXT", nullable: true),
                    CONTROLLER_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    ACTION_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    ROUTE_URL = table.Column<string>(type: "TEXT", nullable: true),
                    ICON_CLASS = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MENU");
        }
    }
}
