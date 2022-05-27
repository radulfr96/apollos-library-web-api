using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedDescriptionToSubType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("670e13af-246c-4c8e-a17f-aa25d23bc10b"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubscriptionTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SubscriptionTypes",
                keyColumn: "SubscriptionTypeId",
                keyValue: 2,
                column: "Description",
                value: "This subscription is for individuals keeping track of their own library.");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("15305563-2441-45c5-b023-0dc7f59d6de7"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubscriptionTypes");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("670e13af-246c-4c8e-a17f-aa25d23bc10b"), new DateTime(2102, 5, 26, 21, 35, 2, 89, DateTimeKind.Local).AddTicks(6342), new DateTime(2022, 5, 26, 21, 35, 2, 89, DateTimeKind.Local).AddTicks(6410), 1 });
        }
    }
}
