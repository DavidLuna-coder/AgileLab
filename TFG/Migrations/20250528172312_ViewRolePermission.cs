using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG.Migrations
{
    /// <inheritdoc />
    public partial class ViewRolePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d3b1f5c2-8e4a-4c0b-9f6e-7c1d2e3f4a5b"),
                column: "Permissions",
                value: 32767);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d3b1f5c2-8e4a-4c0b-9f6e-7c1d2e3f4a5b"),
                column: "Permissions",
                value: 16383);
        }
    }
}
