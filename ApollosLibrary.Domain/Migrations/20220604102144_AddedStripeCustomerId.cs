using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedStripeCustomerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("167841e4-3721-4963-bc22-8eb4a3598074"));

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("93d25dcf-df84-4fdd-bfc5-99269e4d4ed5"), new DateTime(2102, 6, 4, 20, 21, 15, 936, DateTimeKind.Local).AddTicks(9057), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 4, 20, 21, 15, 936, DateTimeKind.Local).AddTicks(9092), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("93d25dcf-df84-4fdd-bfc5-99269e4d4ed5"));

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Subscriptions");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("167841e4-3721-4963-bc22-8eb4a3598074"), new DateTime(2102, 6, 1, 20, 37, 59, 402, DateTimeKind.Local).AddTicks(4172), null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 1, 20, 37, 59, 402, DateTimeKind.Local).AddTicks(4211), 1 });
        }
    }
}
