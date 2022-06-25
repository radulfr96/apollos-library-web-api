using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class RemovedUnneededField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("48dddd49-b15e-4882-ade7-9617a82ad070"));

            migrationBuilder.DropColumn(
                name: "EntryId",
                table: "EntryReports");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("8911140b-b3ca-4d57-be60-d62ef78dd4d2"), new DateTime(2102, 6, 25, 16, 37, 54, 884, DateTimeKind.Local).AddTicks(1169), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 25, 16, 37, 54, 884, DateTimeKind.Local).AddTicks(1202), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("8911140b-b3ca-4d57-be60-d62ef78dd4d2"));

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                table: "EntryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("48dddd49-b15e-4882-ade7-9617a82ad070"), new DateTime(2102, 6, 18, 18, 37, 40, 761, DateTimeKind.Local).AddTicks(7841), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 18, 18, 37, 40, 761, DateTimeKind.Local).AddTicks(7872), 1 });
        }
    }
}
