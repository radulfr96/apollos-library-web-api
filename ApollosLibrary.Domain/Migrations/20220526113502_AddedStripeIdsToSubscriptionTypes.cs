using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedStripeIdsToSubscriptionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("2b3755e8-0c16-496b-98e0-e626791a7acf"));

            migrationBuilder.AddColumn<string>(
                name: "StripePriceId",
                table: "SubscriptionTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "StripePriceId",
                value: "price_1L3eu4HSN4IIrwiZsUfrItzs");

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 3,
                column: "StripePriceId",
                value: "price_1L3euyHSN4IIrwiZvJYhpH2T");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("670e13af-246c-4c8e-a17f-aa25d23bc10b"), new DateTime(2102, 5, 26, 21, 35, 2, 89, DateTimeKind.Local).AddTicks(6342), new DateTime(2022, 5, 26, 21, 35, 2, 89, DateTimeKind.Local).AddTicks(6410), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("670e13af-246c-4c8e-a17f-aa25d23bc10b"));

            migrationBuilder.DropColumn(
                name: "StripePriceId",
                table: "SubscriptionTypes");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("2b3755e8-0c16-496b-98e0-e626791a7acf"), new DateTime(2102, 5, 26, 20, 39, 47, 85, DateTimeKind.Local).AddTicks(2543), new DateTime(2022, 5, 26, 20, 39, 47, 85, DateTimeKind.Local).AddTicks(2579), 1 });
        }
    }
}
