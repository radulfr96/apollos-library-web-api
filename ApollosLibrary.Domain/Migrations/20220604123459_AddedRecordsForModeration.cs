using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedRecordsForModeration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("93d25dcf-df84-4fdd-bfc5-99269e4d4ed5"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("d2e55007-b499-4324-acad-70f399591167"), new DateTime(2102, 6, 4, 22, 34, 32, 368, DateTimeKind.Local).AddTicks(9479), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 4, 22, 34, 32, 368, DateTimeKind.Local).AddTicks(9524), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("d2e55007-b499-4324-acad-70f399591167"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("93d25dcf-df84-4fdd-bfc5-99269e4d4ed5"), new DateTime(2102, 6, 4, 20, 21, 15, 936, DateTimeKind.Local).AddTicks(9057), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 4, 20, 21, 15, 936, DateTimeKind.Local).AddTicks(9092), 1 });
        }
    }
}
