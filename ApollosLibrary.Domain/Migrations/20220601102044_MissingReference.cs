using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class MissingReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("6c12b1da-8b98-4c71-b1fe-df55a7da957a"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("6d8a6039-f74b-436b-8940-2a7d979d9673"), new DateTime(2102, 6, 1, 20, 20, 43, 638, DateTimeKind.Local).AddTicks(2512), null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 1, 20, 20, 43, 638, DateTimeKind.Local).AddTicks(2551), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("6d8a6039-f74b-436b-8940-2a7d979d9673"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("6c12b1da-8b98-4c71-b1fe-df55a7da957a"), new DateTime(2102, 6, 1, 20, 17, 31, 115, DateTimeKind.Local).AddTicks(2644), null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 1, 20, 17, 31, 115, DateTimeKind.Local).AddTicks(2681), 1 });
        }
    }
}
