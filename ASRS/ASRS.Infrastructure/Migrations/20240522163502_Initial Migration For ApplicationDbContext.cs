using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASRS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationForApplicationDbContext : Migration
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
                    LedgerFolioNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMasters", x => x.ItemCode);
                });

            migrationBuilder.CreateTable(
                name: "StoreReceiptRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRVNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerFolioNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CRVDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReceiptRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockReleases",
                columns: table => new
                {
                    StockReleaseNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueVoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SHNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandQuantity = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemCode = table.Column<int>(type: "int", nullable: false),
                    ItemMasterItemCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockReleases", x => x.StockReleaseNo);
                    table.ForeignKey(
                        name: "FK_StockReleases_ItemMasters_ItemMasterItemCode",
                        column: x => x.ItemMasterItemCode,
                        principalTable: "ItemMasters",
                        principalColumn: "ItemCode");
                });

            migrationBuilder.CreateTable(
                name: "StoreReceipts",
                columns: table => new
                {
                    StoreReceiptNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRVNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SHNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false),
                    QuantityStored = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CRVDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemCode = table.Column<int>(type: "int", nullable: false),
                    ItemMasterItemCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReceipts", x => x.StoreReceiptNo);
                    table.ForeignKey(
                        name: "FK_StoreReceipts_ItemMasters_ItemMasterItemCode",
                        column: x => x.ItemMasterItemCode,
                        principalTable: "ItemMasters",
                        principalColumn: "ItemCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockReleases_ItemMasterItemCode",
                table: "StockReleases",
                column: "ItemMasterItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReceipts_ItemMasterItemCode",
                table: "StoreReceipts",
                column: "ItemMasterItemCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockReleases");

            migrationBuilder.DropTable(
                name: "StoreReceiptRequests");

            migrationBuilder.DropTable(
                name: "StoreReceipts");

            migrationBuilder.DropTable(
                name: "ItemMasters");
        }
    }
}
