using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedCreatedFieldsToReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("be281799-290c-4885-a5f7-f232ea6340d2"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntryReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("52b2daef-f937-435a-a06c-dc1c102a1924"), new DateTime(2102, 6, 10, 20, 16, 1, 140, DateTimeKind.Local).AddTicks(9294), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 10, 20, 16, 1, 140, DateTimeKind.Local).AddTicks(9323), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("52b2daef-f937-435a-a06c-dc1c102a1924"));

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntryReports");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("be281799-290c-4885-a5f7-f232ea6340d2"), new DateTime(2102, 6, 8, 19, 30, 8, 461, DateTimeKind.Local).AddTicks(5626), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 8, 19, 30, 8, 461, DateTimeKind.Local).AddTicks(5656), 1 });
        }
    }
}
