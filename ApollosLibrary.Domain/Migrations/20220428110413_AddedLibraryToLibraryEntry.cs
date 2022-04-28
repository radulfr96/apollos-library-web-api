using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedLibraryToLibraryEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("a4f1538b-f4c9-440b-bf1c-98a791bc4ef7"));

            migrationBuilder.AddColumn<int>(
                name: "LibraryId",
                table: "LibraryEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "JoinDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("cab27a35-aeed-429d-a8dd-281129113892"), new DateTime(2102, 4, 28, 21, 4, 12, 320, DateTimeKind.Local).AddTicks(368), new DateTime(2022, 4, 28, 21, 4, 12, 320, DateTimeKind.Local).AddTicks(427), 1 });

            migrationBuilder.UpdateData(
                table: "UserSubscriptions",
                keyColumn: "UserSubscrptionId",
                keyValue: 1,
                column: "SubscriptionId",
                value: new Guid("cab27a35-aeed-429d-a8dd-281129113892"));

            migrationBuilder.CreateIndex(
                name: "IX_LibraryEntries_LibraryId",
                table: "LibraryEntries",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryEntries_Libraries_LibraryId",
                table: "LibraryEntries",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "LibraryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryEntries_Libraries_LibraryId",
                table: "LibraryEntries");

            migrationBuilder.DropIndex(
                name: "IX_LibraryEntries_LibraryId",
                table: "LibraryEntries");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("cab27a35-aeed-429d-a8dd-281129113892"));

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "LibraryEntries");

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
    }
}
