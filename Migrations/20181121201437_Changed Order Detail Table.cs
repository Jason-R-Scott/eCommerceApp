using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceApp.Migrations
{
    public partial class ChangedOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityOrdered",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SubTotal",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "QuantityOrdered");
        }
    }
}
