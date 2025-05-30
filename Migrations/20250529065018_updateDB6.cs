using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCWebApp.Migrations
{
    /// <inheritdoc />
    public partial class updateDB6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarginCall",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientCode = table.Column<string>(type: "TEXT", nullable: false),
                    LedgerBal = table.Column<string>(type: "TEXT", nullable: false),
                    TNE = table.Column<string>(type: "TEXT", nullable: false),
                    IM = table.Column<string>(type: "TEXT", nullable: false),
                    Percentages = table.Column<int>(type: "INTEGER", nullable: false),
                    CcyCode = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeOfMarginCall = table.Column<string>(type: "TEXT", nullable: false),
                    OrderDetails = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStemp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarginCall", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarginCall");
        }
    }
}
