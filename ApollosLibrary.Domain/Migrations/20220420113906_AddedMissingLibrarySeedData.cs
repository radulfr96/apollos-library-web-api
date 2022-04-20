using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedMissingLibrarySeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("70ad9aa8-d5ba-40bb-8330-58ad28508b3c"));

            migrationBuilder.InsertData(
                table: "Libraries",
                columns: new[] { "LibraryId", "UserId" },
                values: new object[] { 1, new Guid("e7f12974-73dd-48d6-aa79-95fe1ded101e") });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("a4f1538b-f4c9-440b-bf1c-98a791bc4ef7"), new DateTime(2102, 4, 20, 21, 39, 5, 883, DateTimeKind.Local).AddTicks(3334), new DateTime(2022, 4, 20, 21, 39, 5, 883, DateTimeKind.Local).AddTicks(3364), 1 });

            migrationBuilder.UpdateData(
                table: "UserSubscriptions",
                keyColumn: "UserSubscrptionId",
                keyValue: 1,
                column: "SubscriptionId",
                value: new Guid("a4f1538b-f4c9-440b-bf1c-98a791bc4ef7"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Libraries",
                keyColumn: "LibraryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("a4f1538b-f4c9-440b-bf1c-98a791bc4ef7"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("70ad9aa8-d5ba-40bb-8330-58ad28508b3c"), new DateTime(2102, 4, 20, 21, 37, 17, 358, DateTimeKind.Local).AddTicks(1579), new DateTime(2022, 4, 20, 21, 37, 17, 358, DateTimeKind.Local).AddTicks(1612), 1 });

            migrationBuilder.UpdateData(
                table: "UserSubscriptions",
                keyColumn: "UserSubscrptionId",
                keyValue: 1,
                column: "SubscriptionId",
                value: new Guid("70ad9aa8-d5ba-40bb-8330-58ad28508b3c"));
        }
    }
}
