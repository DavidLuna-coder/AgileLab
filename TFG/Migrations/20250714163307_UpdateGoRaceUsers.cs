using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoRaceUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bugs",
                table: "UserProjectStatusSnapshot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CodeSmells",
                table: "UserProjectStatusSnapshot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Loc",
                table: "UserProjectStatusSnapshot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Vulnerabilities",
                table: "UserProjectStatusSnapshot",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bugs",
                table: "UserProjectStatusSnapshot");

            migrationBuilder.DropColumn(
                name: "CodeSmells",
                table: "UserProjectStatusSnapshot");

            migrationBuilder.DropColumn(
                name: "Loc",
                table: "UserProjectStatusSnapshot");

            migrationBuilder.DropColumn(
                name: "Vulnerabilities",
                table: "UserProjectStatusSnapshot");
        }
    }
}
