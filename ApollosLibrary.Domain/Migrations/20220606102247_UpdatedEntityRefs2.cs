using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class UpdatedEntityRefs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("e960f3d9-444d-4142-8eb1-f35e3d2649fb"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("64418bc6-fca6-4a21-bab7-a192de20ec1e"), new DateTime(2102, 6, 6, 20, 22, 34, 0, DateTimeKind.Local).AddTicks(2420), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 22, 34, 0, DateTimeKind.Local).AddTicks(2479), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("64418bc6-fca6-4a21-bab7-a192de20ec1e"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("e960f3d9-444d-4142-8eb1-f35e3d2649fb"), new DateTime(2102, 6, 6, 20, 21, 13, 45, DateTimeKind.Local).AddTicks(1142), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 21, 13, 45, DateTimeKind.Local).AddTicks(1178), 1 });
        }
    }
}
