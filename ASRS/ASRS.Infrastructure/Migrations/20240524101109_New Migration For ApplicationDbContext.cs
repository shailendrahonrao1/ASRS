using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASRS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationForApplicationDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreReceiptRequests");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "StoreReceipts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "StockReleases",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ItemCode",
                table: "StoreReceipts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemCode",
                table: "StockReleases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StoreReceiptRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRVDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CRVNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerFolioNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReceiptRequests", x => x.Id);
                });
        }
    }
}
