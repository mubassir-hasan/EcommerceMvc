using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_MVC_Core.Migrations
{
    public partial class mig_productcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Users_UsersId",
                table: "ProductComments");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ProductComments",
                newName: "UsersId1");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_UsersId",
                table: "ProductComments",
                newName: "IX_ProductComments_UsersId1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 20, 23, 11, 50, 118, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 9, 12, 36, 1, 992, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 20, 23, 11, 50, 140, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 9, 12, 36, 2, 5, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Users_UsersId1",
                table: "ProductComments",
                column: "UsersId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Users_UsersId1",
                table: "ProductComments");

            migrationBuilder.RenameColumn(
                name: "UsersId1",
                table: "ProductComments",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_UsersId1",
                table: "ProductComments",
                newName: "IX_ProductComments_UsersId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "ProductLikes",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 9, 12, 36, 1, 992, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 20, 23, 11, 50, 118, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "OrderStatus",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 9, 12, 36, 2, 5, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 20, 23, 11, 50, 140, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Users_UsersId",
                table: "ProductComments",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
