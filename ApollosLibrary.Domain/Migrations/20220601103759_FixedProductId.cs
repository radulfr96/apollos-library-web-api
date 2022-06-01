using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class FixedProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("6d8a6039-f74b-436b-8940-2a7d979d9673"));

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "StripeProductId",
                value: "prod_LlBGpg7ytim1dy");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("167841e4-3721-4963-bc22-8eb4a3598074"), new DateTime(2102, 6, 1, 20, 37, 59, 402, DateTimeKind.Local).AddTicks(4172), null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 1, 20, 37, 59, 402, DateTimeKind.Local).AddTicks(4211), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("167841e4-3721-4963-bc22-8eb4a3598074"));

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "StripeProductId",
                value: "price_1L3eu4HSN4IIrwiZsUfrItzs");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("6d8a6039-f74b-436b-8940-2a7d979d9673"), new DateTime(2102, 6, 1, 20, 20, 43, 638, DateTimeKind.Local).AddTicks(2512), null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 1, 20, 20, 43, 638, DateTimeKind.Local).AddTicks(2551), 1 });
        }
    }
}
