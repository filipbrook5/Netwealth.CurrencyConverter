using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netwealth.Dal.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromCurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "Decimal(20,14)", nullable: false),
                    InverseRate = table.Column<decimal>(type: "Decimal(20,14)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
