using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class MissingOrderConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("5c789bdf-7e5c-4f85-8852-39794c678605"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("e3cf74b3-99ba-428b-8d25-f95fbbca407c"), new DateTime(2102, 5, 14, 17, 56, 38, 762, DateTimeKind.Local).AddTicks(532), new DateTime(2022, 5, 14, 17, 56, 38, 762, DateTimeKind.Local).AddTicks(568), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("e3cf74b3-99ba-428b-8d25-f95fbbca407c"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("5c789bdf-7e5c-4f85-8852-39794c678605"), new DateTime(2102, 5, 10, 21, 7, 52, 404, DateTimeKind.Local).AddTicks(9909), new DateTime(2022, 5, 10, 21, 7, 52, 404, DateTimeKind.Local).AddTicks(9940), 1 });
        }
    }
}
