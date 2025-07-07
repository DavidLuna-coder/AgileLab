using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class ProjectsSnapshots_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectStatusSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bugs = table.Column<int>(type: "int", nullable: false),
                    Vulnerabilities = table.Column<int>(type: "int", nullable: false),
                    CodeSmells = table.Column<int>(type: "int", nullable: false),
                    Loc = table.Column<int>(type: "int", nullable: false),
                    ClosedTasks = table.Column<int>(type: "int", nullable: false),
                    OpenTasks = table.Column<int>(type: "int", nullable: false),
                    OverdueTasks = table.Column<int>(type: "int", nullable: false),
                    MergeRequestsCreated = table.Column<int>(type: "int", nullable: false),
                    MergeRequestsMerged = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatusSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStatusSnapshots_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProjectStatusSnapshot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedTasks = table.Column<int>(type: "int", nullable: false),
                    OpenTasks = table.Column<int>(type: "int", nullable: false),
                    OverdueTasks = table.Column<int>(type: "int", nullable: false),
                    MergeRequestsCreated = table.Column<int>(type: "int", nullable: false),
                    MergeRequestsMerged = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjectStatusSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProjectStatusSnapshot_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjectStatusSnapshot_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatusSnapshots_ProjectId",
                table: "ProjectStatusSnapshots",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectStatusSnapshot_ProjectId",
                table: "UserProjectStatusSnapshot",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectStatusSnapshot_UserId",
                table: "UserProjectStatusSnapshot",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectStatusSnapshots");

            migrationBuilder.DropTable(
                name: "UserProjectStatusSnapshot");
        }
    }
}
