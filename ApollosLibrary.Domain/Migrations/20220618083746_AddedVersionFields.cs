using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedVersionFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("d02c60f2-21a7-44fc-a149-ab9b1cb4c789"));

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "Series",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryRecordId",
                table: "EntryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "Business",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("48dddd49-b15e-4882-ade7-9617a82ad070"), new DateTime(2102, 6, 18, 18, 37, 40, 761, DateTimeKind.Local).AddTicks(7841), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 18, 18, 37, 40, 761, DateTimeKind.Local).AddTicks(7872), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("48dddd49-b15e-4882-ade7-9617a82ad070"));

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "EntryRecordId",
                table: "EntryReports");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Authors");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("d02c60f2-21a7-44fc-a149-ab9b1cb4c789"), new DateTime(2102, 6, 11, 22, 50, 23, 148, DateTimeKind.Local).AddTicks(1805), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 11, 22, 50, 23, 148, DateTimeKind.Local).AddTicks(1899), 1 });
        }
    }
}
