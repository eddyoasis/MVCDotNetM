using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCWebApp.Migrations
{
    /// <inheritdoc />
    public partial class updateMrginCallTableAddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "MarginCall",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "MarginCall",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "MarginCall");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "MarginCall");
        }
    }
}
