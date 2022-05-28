using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class ChangedPriceIdsToProductIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("c03b4b46-3f09-4bf6-b03c-cc87a89c339b"));

            migrationBuilder.RenameColumn(
                name: "StripePriceId",
                table: "SubscriptionTypes",
                newName: "StripeProductId");

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "StripeProductId",
                value: "prod_LlBGpg7ytim1dy");

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 3,
                column: "StripeProductId",
                value: "prod_LlBHeWO1QAe9Dx");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("9752376a-ded1-42bb-a3a7-f70dd069a5a1"), new DateTime(2102, 5, 28, 17, 30, 17, 516, DateTimeKind.Local).AddTicks(2405), new DateTime(2022, 5, 28, 17, 30, 17, 516, DateTimeKind.Local).AddTicks(2438), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("9752376a-ded1-42bb-a3a7-f70dd069a5a1"));

            migrationBuilder.RenameColumn(
                name: "StripeProductId",
                table: "SubscriptionTypes",
                newName: "StripePriceId");

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
                values: new object[] { new Guid("c03b4b46-3f09-4bf6-b03c-cc87a89c339b"), new DateTime(2102, 5, 27, 23, 44, 3, 72, DateTimeKind.Local).AddTicks(4158), new DateTime(2022, 5, 27, 23, 44, 3, 72, DateTimeKind.Local).AddTicks(4196), 1 });
        }
    }
}
