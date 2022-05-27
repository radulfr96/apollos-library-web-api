using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class FixedDescriptionToSubType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("15305563-2441-45c5-b023-0dc7f59d6de7"));

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 3,
                column: "Description",
                value: "This subscription is for families keeping track of their own libraries. Each user will have their own library.");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("c03b4b46-3f09-4bf6-b03c-cc87a89c339b"), new DateTime(2102, 5, 27, 23, 44, 3, 72, DateTimeKind.Local).AddTicks(4158), new DateTime(2022, 5, 27, 23, 44, 3, 72, DateTimeKind.Local).AddTicks(4196), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("c03b4b46-3f09-4bf6-b03c-cc87a89c339b"));

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 3,
                column: "Description",
                value: "This subscription is for families keeping track of their own libraries.Each user will have their own library.");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("15305563-2441-45c5-b023-0dc7f59d6de7"), new DateTime(2102, 5, 27, 23, 37, 5, 489, DateTimeKind.Local).AddTicks(7020), new DateTime(2022, 5, 27, 23, 37, 5, 489, DateTimeKind.Local).AddTicks(7090), 1 });
        }
    }
}
