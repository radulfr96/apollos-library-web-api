using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AdjustmentsToSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("9752376a-ded1-42bb-a3a7-f70dd069a5a1"));

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Subscriptions",
                newName: "SubscriptionDate");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionAdmin",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "StripeProductId",
                value: "price_1L3eu4HSN4IIrwiZsUfrItzs");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("6c12b1da-8b98-4c71-b1fe-df55a7da957a"), new DateTime(2102, 6, 1, 20, 17, 31, 115, DateTimeKind.Local).AddTicks(2644), null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 1, 20, 17, 31, 115, DateTimeKind.Local).AddTicks(2681), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("6c12b1da-8b98-4c71-b1fe-df55a7da957a"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionAdmin",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionDate",
                table: "Subscriptions",
                newName: "JoinDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "StripeProductId",
                value: "prod_LlBGpg7ytim1dy");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("9752376a-ded1-42bb-a3a7-f70dd069a5a1"), new DateTime(2102, 5, 28, 17, 30, 17, 516, DateTimeKind.Local).AddTicks(2405), new DateTime(2022, 5, 28, 17, 30, 17, 516, DateTimeKind.Local).AddTicks(2438), 1 });
        }
    }
}
