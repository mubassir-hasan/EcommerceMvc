using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_MVC_Core.Migrations
{
    public partial class Mig_OrderDetailsChangedOneToOneToOneTOMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 84, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 21, 0, 3, 21, 364, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 99, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 21, 0, 3, 21, 411, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 21, 0, 3, 21, 364, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 84, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 21, 0, 3, 21, 411, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 99, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                unique: true);
        }
    }
}
