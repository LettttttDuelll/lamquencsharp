using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNPT.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USERNAME = table.Column<string>(type: "TEXT", nullable: true),
                    PASSWORD = table.Column<string>(type: "TEXT", nullable: true),
                    FULLNAME = table.Column<string>(type: "TEXT", nullable: true),
                    ISDELETED = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
