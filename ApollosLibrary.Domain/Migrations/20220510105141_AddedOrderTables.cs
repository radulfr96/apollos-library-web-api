using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedOrderTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Libraries",
                keyColumn: "LibraryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserSubscriptions",
                keyColumn: "UserSubscrptionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("cab27a35-aeed-429d-a8dd-281129113892"));

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyRate",
                table: "SubscriptionTypes",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("cf376d85-4155-49f0-91a1-81de73021ba6"), new DateTime(2102, 5, 10, 20, 51, 41, 347, DateTimeKind.Local).AddTicks(4939), new DateTime(2022, 5, 10, 20, 51, 41, 347, DateTimeKind.Local).AddTicks(4974), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("cf376d85-4155-49f0-91a1-81de73021ba6"));

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyRate",
                table: "SubscriptionTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.InsertData(
                table: "Libraries",
                columns: new[] { "LibraryId", "UserId" },
                values: new object[] { 1, new Guid("e7f12974-73dd-48d6-aa79-95fe1ded101e") });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("cab27a35-aeed-429d-a8dd-281129113892"), new DateTime(2102, 4, 28, 21, 4, 12, 320, DateTimeKind.Local).AddTicks(368), new DateTime(2022, 4, 28, 21, 4, 12, 320, DateTimeKind.Local).AddTicks(427), 1 });

            migrationBuilder.InsertData(
                table: "UserSubscriptions",
                columns: new[] { "UserSubscrptionId", "SubscriptionId", "UserId" },
                values: new object[] { 1, new Guid("cab27a35-aeed-429d-a8dd-281129113892"), new Guid("e7f12974-73dd-48d6-aa79-95fe1ded101e") });
        }
    }
}
