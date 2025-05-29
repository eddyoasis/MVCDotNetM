using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCWebApp.Migrations
{
    /// <inheritdoc />
    public partial class updateDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarginFormula",
                table: "EmailNotifications",
                newName: "EmailTemplate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailTemplate",
                table: "EmailNotifications",
                newName: "MarginFormula");
        }
    }
}
