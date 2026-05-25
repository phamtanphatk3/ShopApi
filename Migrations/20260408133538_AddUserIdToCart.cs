using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Username] = N'admin')
                BEGIN
                    INSERT INTO [Users] ([Username], [Password], [Role])
                    VALUES (N'admin', N'123', N'Admin');
                END;

                IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Username] = N'user1')
                BEGIN
                    INSERT INTO [Users] ([Username], [Password], [Role])
                    VALUES (N'user1', N'123', N'Customer');
                END;

                DECLARE @DefaultUserId INT = (SELECT TOP(1) [Id] FROM [Users] ORDER BY [Id]);

                UPDATE [Carts]
                SET [UserId] = @DefaultUserId
                WHERE [UserId] = 0;

                UPDATE [Orders]
                SET [UserId] = @DefaultUserId
                WHERE [UserId] = 0;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Carts");
        }
    }
}
