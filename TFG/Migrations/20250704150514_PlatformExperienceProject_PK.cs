using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class PlatformExperienceProject_PK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoRacePlatformExperienceProject_GoRaceExperiences_PlatformExperiencesId",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropForeignKey(
                name: "FK_GoRacePlatformExperienceProject_Projects_ProjectsId",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoRacePlatformExperienceProject",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.RenameColumn(
                name: "ProjectsId",
                table: "GoRacePlatformExperienceProject",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "PlatformExperiencesId",
                table: "GoRacePlatformExperienceProject",
                newName: "GoRacePlatformExperienceId");

            migrationBuilder.RenameIndex(
                name: "IX_GoRacePlatformExperienceProject_ProjectsId",
                table: "GoRacePlatformExperienceProject",
                newName: "IX_GoRacePlatformExperienceProject_ProjectId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GoRacePlatformExperienceProject",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "GoRacePlatformExperienceProject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoRacePlatformExperienceProject",
                table: "GoRacePlatformExperienceProject",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GoRacePlatformExperienceProject_GoRacePlatformExperienceId",
                table: "GoRacePlatformExperienceProject",
                column: "GoRacePlatformExperienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoRacePlatformExperienceProject_GoRaceExperiences_GoRacePlatformExperienceId",
                table: "GoRacePlatformExperienceProject",
                column: "GoRacePlatformExperienceId",
                principalTable: "GoRaceExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoRacePlatformExperienceProject_Projects_ProjectId",
                table: "GoRacePlatformExperienceProject",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoRacePlatformExperienceProject_GoRaceExperiences_GoRacePlatformExperienceId",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropForeignKey(
                name: "FK_GoRacePlatformExperienceProject_Projects_ProjectId",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoRacePlatformExperienceProject",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropIndex(
                name: "IX_GoRacePlatformExperienceProject_GoRacePlatformExperienceId",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "GoRacePlatformExperienceProject");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "GoRacePlatformExperienceProject",
                newName: "ProjectsId");

            migrationBuilder.RenameColumn(
                name: "GoRacePlatformExperienceId",
                table: "GoRacePlatformExperienceProject",
                newName: "PlatformExperiencesId");

            migrationBuilder.RenameIndex(
                name: "IX_GoRacePlatformExperienceProject_ProjectId",
                table: "GoRacePlatformExperienceProject",
                newName: "IX_GoRacePlatformExperienceProject_ProjectsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoRacePlatformExperienceProject",
                table: "GoRacePlatformExperienceProject",
                columns: new[] { "PlatformExperiencesId", "ProjectsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GoRacePlatformExperienceProject_GoRaceExperiences_PlatformExperiencesId",
                table: "GoRacePlatformExperienceProject",
                column: "PlatformExperiencesId",
                principalTable: "GoRaceExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoRacePlatformExperienceProject_Projects_ProjectsId",
                table: "GoRacePlatformExperienceProject",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
