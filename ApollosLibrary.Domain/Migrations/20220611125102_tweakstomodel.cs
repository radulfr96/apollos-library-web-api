using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class tweakstomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntryReports_EntryReportType_TypeEntryReportTypeId",
                table: "EntryReports");

            migrationBuilder.DropIndex(
                name: "IX_EntryReports_TypeEntryReportTypeId",
                table: "EntryReports");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("52eb0d74-a638-499c-8a78-8db14d91d3b2"));

            migrationBuilder.DropColumn(
                name: "TypeEntryReportTypeId",
                table: "EntryReports");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("d02c60f2-21a7-44fc-a149-ab9b1cb4c789"), new DateTime(2102, 6, 11, 22, 50, 23, 148, DateTimeKind.Local).AddTicks(1805), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 11, 22, 50, 23, 148, DateTimeKind.Local).AddTicks(1899), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_EntryTypeId",
                table: "EntryReports",
                column: "EntryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryReports_EntryReportType_EntryTypeId",
                table: "EntryReports",
                column: "EntryTypeId",
                principalTable: "EntryReportType",
                principalColumn: "EntryReportTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntryReports_EntryReportType_EntryTypeId",
                table: "EntryReports");

            migrationBuilder.DropIndex(
                name: "IX_EntryReports_EntryTypeId",
                table: "EntryReports");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("d02c60f2-21a7-44fc-a149-ab9b1cb4c789"));

            migrationBuilder.AddColumn<int>(
                name: "TypeEntryReportTypeId",
                table: "EntryReports",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("52eb0d74-a638-499c-8a78-8db14d91d3b2"), new DateTime(2102, 6, 11, 20, 24, 39, 716, DateTimeKind.Local).AddTicks(4729), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 11, 20, 24, 39, 716, DateTimeKind.Local).AddTicks(4761), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_TypeEntryReportTypeId",
                table: "EntryReports",
                column: "TypeEntryReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryReports_EntryReportType_TypeEntryReportTypeId",
                table: "EntryReports",
                column: "TypeEntryReportTypeId",
                principalTable: "EntryReportType",
                principalColumn: "EntryReportTypeId");
        }
    }
}
