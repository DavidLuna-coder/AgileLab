using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class ProjectChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AspNetUsers");
        }
    }
}
