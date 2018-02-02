using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ecommerce_MVC_Core.Migrations
{
    public partial class mig_orderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2018, 1, 19, 2, 51, 34, 517, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 1, 19, 2, 7, 59, 464, DateTimeKind.Local));

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusId",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UsersId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatus_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderStatus_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStatusId",
                table: "Order",
                column: "OrderStatusId",
                unique: true,
                filter: "[OrderStatusId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_StatusId",
                table: "OrderStatus",
                column: "StatusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_UsersId",
                table: "OrderStatus",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderStatus_OrderStatusId",
                table: "Order",
                column: "OrderStatusId",
                principalTable: "OrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderStatus_OrderStatusId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderStatusId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderStatusId",
                table: "Order");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2018, 1, 19, 2, 7, 59, 464, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 1, 19, 2, 51, 34, 517, DateTimeKind.Local));
        }
    }
}
