using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class GoRaceExperience_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoRaceExperiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExperienceType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoRaceExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoRaceExperiences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoRacePlatformExperienceProject",
                columns: table => new
                {
                    GoRacePlatformExperienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoRacePlatformExperienceProject", x => new { x.GoRacePlatformExperienceId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_GoRacePlatformExperienceProject_GoRaceExperiences_GoRacePlatformExperienceId",
                        column: x => x.GoRacePlatformExperienceId,
                        principalTable: "GoRaceExperiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoRacePlatformExperienceProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoRaceExperiences_ProjectId",
                table: "GoRaceExperiences",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GoRacePlatformExperienceProject_ProjectsId",
                table: "GoRacePlatformExperienceProject",
                column: "ProjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoRacePlatformExperienceProject");

            migrationBuilder.DropTable(
                name: "GoRaceExperiences");
        }
    }
}
