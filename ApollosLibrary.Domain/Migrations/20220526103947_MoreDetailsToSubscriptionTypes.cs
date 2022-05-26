using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class MoreDetailsToSubscriptionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("e3cf74b3-99ba-428b-8d25-f95fbbca407c"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "SubscriptionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxUsers",
                table: "SubscriptionTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Purchasable",
                table: "SubscriptionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 1,
                columns: new[] { "IsAvailable", "MaxUsers" },
                values: new object[] { true, 1 });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                columns: new[] { "IsAvailable", "MaxUsers", "Purchasable" },
                values: new object[] { true, 1, true });

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 3,
                columns: new[] { "MaxUsers", "Purchasable" },
                values: new object[] { 5, true });

            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "SubscriptionTypeId", "IsAvailable", "MaxUsers", "MonthlyRate", "Purchasable", "SubscriptionName" },
                values: new object[] { -1, true, 1, 0.00m, false, "Signed Up" });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("2b3755e8-0c16-496b-98e0-e626791a7acf"), new DateTime(2102, 5, 26, 20, 39, 47, 85, DateTimeKind.Local).AddTicks(2543), new DateTime(2022, 5, 26, 20, 39, 47, 85, DateTimeKind.Local).AddTicks(2579), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("2b3755e8-0c16-496b-98e0-e626791a7acf"));

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "SubscriptionTypes");

            migrationBuilder.DropColumn(
                name: "MaxUsers",
                table: "SubscriptionTypes");

            migrationBuilder.DropColumn(
                name: "Purchasable",
                table: "SubscriptionTypes");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("e3cf74b3-99ba-428b-8d25-f95fbbca407c"), new DateTime(2102, 5, 14, 17, 56, 38, 762, DateTimeKind.Local).AddTicks(532), new DateTime(2022, 5, 14, 17, 56, 38, 762, DateTimeKind.Local).AddTicks(568), 1 });
        }
    }
}
