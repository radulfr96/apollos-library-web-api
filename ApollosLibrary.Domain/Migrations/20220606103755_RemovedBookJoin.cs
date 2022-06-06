using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class RemovedBookJoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecords_Books_BookId",
                table: "BookRecords");

            migrationBuilder.DropIndex(
                name: "IX_BookRecords_BookId",
                table: "BookRecords");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("64418bc6-fca6-4a21-bab7-a192de20ec1e"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("16d6fda8-a9a0-4cb4-bd17-86bdd2286554"), new DateTime(2102, 6, 6, 20, 37, 51, 355, DateTimeKind.Local).AddTicks(6459), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 37, 51, 355, DateTimeKind.Local).AddTicks(6496), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("16d6fda8-a9a0-4cb4-bd17-86bdd2286554"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("64418bc6-fca6-4a21-bab7-a192de20ec1e"), new DateTime(2102, 6, 6, 20, 22, 34, 0, DateTimeKind.Local).AddTicks(2420), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 22, 34, 0, DateTimeKind.Local).AddTicks(2479), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_BookId",
                table: "BookRecords",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecords_Books_BookId",
                table: "BookRecords",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
