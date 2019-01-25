using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceApp.Migrations
{
    public partial class AddedUsertoCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserID",
                table: "Items",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_UserID",
                table: "Items",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_UserID",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UserID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Items");
        }
    }
}
