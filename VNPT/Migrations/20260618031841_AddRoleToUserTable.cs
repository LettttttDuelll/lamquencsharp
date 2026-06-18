using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VNPT.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ROLE",
                table: "USERS",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ROLE",
                table: "USERS");
        }
    }
}
