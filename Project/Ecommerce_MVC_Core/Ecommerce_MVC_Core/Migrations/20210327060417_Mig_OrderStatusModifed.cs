using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_MVC_Core.Migrations
{
    public partial class Mig_OrderStatusModifed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_StatusId",
                table: "OrderStatus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 27, 12, 4, 17, 693, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 84, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 27, 12, 4, 17, 710, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 99, DateTimeKind.Local));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderStatus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_StatusId",
                table: "OrderStatus",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_StatusId",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderStatus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 84, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 27, 12, 4, 17, 693, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 26, 0, 27, 39, 99, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 27, 12, 4, 17, 710, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_StatusId",
                table: "OrderStatus",
                column: "StatusId",
                unique: true);
        }
    }
}
