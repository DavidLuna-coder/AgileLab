using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class Config_Experiences_Factors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImprovementScoreFactor",
                table: "GoRaceExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxOnTimeTasksScore",
                table: "GoRaceExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxQualityScore",
                table: "GoRaceExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImprovementScoreFactor",
                table: "GoRaceExperiences");

            migrationBuilder.DropColumn(
                name: "MaxOnTimeTasksScore",
                table: "GoRaceExperiences");

            migrationBuilder.DropColumn(
                name: "MaxQualityScore",
                table: "GoRaceExperiences");
        }
    }
}
