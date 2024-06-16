using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASRS.Infrastructure.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class NewMigrationForAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "89f64b2e-c31e-4109-83cd-d6d8415bd4ef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ccaed73c-d895-4880-8b7d-0717a7ca0fbc", "AQAAAAIAAYagAAAAECa4YN76moinwzVmI4fs4GfyL3Dh4jYZvlR5WUPOxg37GnLe16vocvSfzigtyU6LYA==", "8451f41d-2cc3-405d-b8cc-94d5d7f6cd20" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "89f64b2e-c31e-4109-83cd-d6d8415bd4ef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "88b8f4fd-d6c5-4901-b0b4-82bbfc8e1ad9", "AQAAAAIAAYagAAAAEBfn2FzdIi+u/KtLdP6nEWTTVdFJF3kyoaWoI9wtU6LLyKwV/qC0m5yZEL+CZALtIg==", "bc42bb5d-9c0c-4824-bcfe-26c45eed66c7" });
        }
    }
}
