using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class AlignWarrantyColumnsWithRequirement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WarrantyRecords");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "WarrantyRecords",
                newName: "WarrantyStartDate");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "WarrantyRecords",
                newName: "WarrantyEndDate");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WarrantyRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE WarrantyRecords
                SET Status = CASE
                    WHEN WarrantyEndDate >= GETUTCDATE() THEN 'InWarranty'
                    ELSE 'Expired'
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "WarrantyRecords");

            migrationBuilder.RenameColumn(
                name: "WarrantyStartDate",
                table: "WarrantyRecords",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "WarrantyEndDate",
                table: "WarrantyRecords",
                newName: "ExpiryDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WarrantyRecords",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
