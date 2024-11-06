using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class RevertedProjectUsersList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersIds",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsersIds",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
