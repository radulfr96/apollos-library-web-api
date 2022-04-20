using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedMissingSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("70ad9aa8-d5ba-40bb-8330-58ad28508b3c"), new DateTime(2102, 4, 20, 21, 37, 17, 358, DateTimeKind.Local).AddTicks(1579), new DateTime(2022, 4, 20, 21, 37, 17, 358, DateTimeKind.Local).AddTicks(1612), 1 });

            migrationBuilder.InsertData(
                table: "UserSubscriptions",
                columns: new[] { "UserSubscrptionId", "SubscriptionId", "UserId" },
                values: new object[] { 1, new Guid("70ad9aa8-d5ba-40bb-8330-58ad28508b3c"), new Guid("e7f12974-73dd-48d6-aa79-95fe1ded101e") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserSubscriptions",
                keyColumn: "UserSubscrptionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("70ad9aa8-d5ba-40bb-8330-58ad28508b3c"));
        }
    }
}
