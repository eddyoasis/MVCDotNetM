using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCWebApp.Migrations
{
    /// <inheritdoc />
    public partial class initCreateToRemoteServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarginType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotifications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MarginCall",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerBal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TNE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentages = table.Column<int>(type: "int", nullable: false),
                    CcyCode = table.Column<int>(type: "int", nullable: false),
                    TypeOfMarginCall = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStemp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarginCall", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MarginFormulas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarginType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Formula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarginFormulas", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotifications");

            migrationBuilder.DropTable(
                name: "MarginCall");

            migrationBuilder.DropTable(
                name: "MarginFormulas");
        }
    }
}
