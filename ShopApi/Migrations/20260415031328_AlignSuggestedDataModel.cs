using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class AlignSuggestedDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Stores",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "ProductImages",
                newName: "IsMain");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "FinalAmount");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Coupons",
                newName: "EndDate");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscountType",
                table: "Promotions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "Promotions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiscountType",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "Coupons",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Coupons",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Carts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "dien-thoai");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "laptop");

            migrationBuilder.Sql(@"
UPDATE c
SET c.Slug = CONCAT('category-', c.Id)
FROM Categories c
WHERE c.Slug IS NULL OR LTRIM(RTRIM(c.Slug)) = '';

UPDATE Promotions
SET DiscountType = CASE
    WHEN DiscountPercent IS NOT NULL THEN 'Percent'
    WHEN DiscountAmount IS NOT NULL THEN 'Amount'
    ELSE 'Amount'
END,
DiscountValue = CASE
    WHEN DiscountPercent IS NOT NULL THEN DiscountPercent
    WHEN DiscountAmount IS NOT NULL THEN DiscountAmount
    ELSE 0
END;

UPDATE Coupons
SET DiscountType = CASE
    WHEN DiscountPercent IS NOT NULL THEN 'Percent'
    WHEN DiscountAmount IS NOT NULL THEN 'Amount'
    ELSE 'Amount'
END,
DiscountValue = CASE
    WHEN DiscountPercent IS NOT NULL THEN DiscountPercent
    WHEN DiscountAmount IS NOT NULL THEN DiscountAmount
    ELSE 0
END;

UPDATE OrderItems
SET LineTotal = UnitPrice * Quantity;

UPDATE o
SET o.CustomerName = u.Username,
    o.OrderCode = CONCAT('ORD-LEGACY-', o.Id)
FROM Orders o
LEFT JOIN Users u ON u.Id = o.UserId;
");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LineTotal",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Stores",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "IsMain",
                table: "ProductImages",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "FinalAmount",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Coupons",
                newName: "ExpiryDate");
        }
    }
}
