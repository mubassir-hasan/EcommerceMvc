using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ecommerce_MVC_Core.Migrations
{
    public partial class mig_Changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderStatus_OrderStatusId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_AspNetUsers_UsersId",
                table: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_UsersId",
                table: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderStatusId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "OrderStatusId",
                table: "Order");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2018, 1, 20, 22, 20, 32, 856, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 1, 19, 2, 51, 34, 517, DateTimeKind.Local));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OrderStatus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2018, 1, 20, 22, 20, 32, 858, DateTimeKind.Local),
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_UserId",
                table: "OrderStatus",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Order_OrderId",
                table: "OrderStatus",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_AspNetUsers_UserId",
                table: "OrderStatus",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Order_OrderId",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_AspNetUsers_UserId",
                table: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatus_UserId",
                table: "OrderStatus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2018, 1, 19, 2, 51, 34, 517, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 1, 20, 22, 20, 32, 856, DateTimeKind.Local));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OrderStatus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 1, 20, 22, 20, 32, 858, DateTimeKind.Local));

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "OrderStatus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusId",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_UsersId",
                table: "OrderStatus",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStatusId",
                table: "Order",
                column: "OrderStatusId",
                unique: true,
                filter: "[OrderStatusId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderStatus_OrderStatusId",
                table: "Order",
                column: "OrderStatusId",
                principalTable: "OrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_AspNetUsers_UsersId",
                table: "OrderStatus",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
