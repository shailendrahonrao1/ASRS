using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASRS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemMasters",
                columns: table => new
                {
                    ItemCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DenominationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerFolioNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMasters", x => x.ItemCode);
                });

            migrationBuilder.CreateTable(
                name: "StockReleases",
                columns: table => new
                {
                    StockReleaseNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueVoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SHNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DemandQuantity = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DemandDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockReleases", x => x.StockReleaseNo);
                });

            migrationBuilder.CreateTable(
                name: "StoreReceipts",
                columns: table => new
                {
                    StoreReceiptNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRVNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SHNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false),
                    QuantityStored = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CRVDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReceipts", x => x.StoreReceiptNo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemMasters");

            migrationBuilder.DropTable(
                name: "StockReleases");

            migrationBuilder.DropTable(
                name: "StoreReceipts");
        }
    }
}
