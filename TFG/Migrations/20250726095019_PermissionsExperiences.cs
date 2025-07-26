using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class PermissionsExperiences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("85a374fa-242a-4323-8b31-32165e0b8e44"),
                column: "Permissions",
                value: 950139);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d3b1f5c2-8e4a-4c0b-9f6e-7c1d2e3f4a5b"),
                column: "Permissions",
                value: 1048575);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("85a374fa-242a-4323-8b31-32165e0b8e44"),
                column: "Permissions",
                value: 475131);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d3b1f5c2-8e4a-4c0b-9f6e-7c1d2e3f4a5b"),
                column: "Permissions",
                value: 65535);
        }
    }
}
