using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class RemovedUnneededFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("f6f13bbe-78cb-458c-ae75-0aaa3e14223c"));

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Books");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("699aca95-5c3b-49bc-a7ea-05fef6b200b1"), new DateTime(2102, 6, 7, 20, 21, 0, 229, DateTimeKind.Local).AddTicks(5529), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 7, 20, 21, 0, 229, DateTimeKind.Local).AddTicks(5573), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("699aca95-5c3b-49bc-a7ea-05fef6b200b1"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("f6f13bbe-78cb-458c-ae75-0aaa3e14223c"), new DateTime(2102, 6, 6, 22, 8, 59, 248, DateTimeKind.Local).AddTicks(3178), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 22, 8, 59, 248, DateTimeKind.Local).AddTicks(3211), 1 });
        }
    }
}
