using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AdjustedIdFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("52b2daef-f937-435a-a06c-dc1c102a1924"));

            migrationBuilder.RenameColumn(
                name: "EntryType",
                table: "EntryReports",
                newName: "EntryTypeId");

            migrationBuilder.AddColumn<int>(
                name: "EntryReportStatusId",
                table: "EntryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeEntryReportTypeId",
                table: "EntryReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EntryReportStatus",
                columns: table => new
                {
                    EntryReportStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReportStatus", x => x.EntryReportStatusId);
                });

            migrationBuilder.CreateTable(
                name: "EntryReportType",
                columns: table => new
                {
                    EntryReportTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReportType", x => x.EntryReportTypeId);
                });

            migrationBuilder.InsertData(
                table: "EntryReportStatus",
                columns: new[] { "EntryReportStatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "Cancelled" },
                    { 3, "Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "EntryReportType",
                columns: new[] { "EntryReportTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Business" },
                    { 2, "Author" },
                    { 3, "Book" },
                    { 4, "Series" }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("52eb0d74-a638-499c-8a78-8db14d91d3b2"), new DateTime(2102, 6, 11, 20, 24, 39, 716, DateTimeKind.Local).AddTicks(4729), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 11, 20, 24, 39, 716, DateTimeKind.Local).AddTicks(4761), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_EntryReportStatusId",
                table: "EntryReports",
                column: "EntryReportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_TypeEntryReportTypeId",
                table: "EntryReports",
                column: "TypeEntryReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryReports_EntryReportStatus_EntryReportStatusId",
                table: "EntryReports",
                column: "EntryReportStatusId",
                principalTable: "EntryReportStatus",
                principalColumn: "EntryReportStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryReports_EntryReportType_TypeEntryReportTypeId",
                table: "EntryReports",
                column: "TypeEntryReportTypeId",
                principalTable: "EntryReportType",
                principalColumn: "EntryReportTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntryReports_EntryReportStatus_EntryReportStatusId",
                table: "EntryReports");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryReports_EntryReportType_TypeEntryReportTypeId",
                table: "EntryReports");

            migrationBuilder.DropTable(
                name: "EntryReportStatus");

            migrationBuilder.DropTable(
                name: "EntryReportType");

            migrationBuilder.DropIndex(
                name: "IX_EntryReports_EntryReportStatusId",
                table: "EntryReports");

            migrationBuilder.DropIndex(
                name: "IX_EntryReports_TypeEntryReportTypeId",
                table: "EntryReports");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("52eb0d74-a638-499c-8a78-8db14d91d3b2"));

            migrationBuilder.DropColumn(
                name: "EntryReportStatusId",
                table: "EntryReports");

            migrationBuilder.DropColumn(
                name: "TypeEntryReportTypeId",
                table: "EntryReports");

            migrationBuilder.RenameColumn(
                name: "EntryTypeId",
                table: "EntryReports",
                newName: "EntryType");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("52b2daef-f937-435a-a06c-dc1c102a1924"), new DateTime(2102, 6, 10, 20, 16, 1, 140, DateTimeKind.Local).AddTicks(9294), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 10, 20, 16, 1, 140, DateTimeKind.Local).AddTicks(9323), 1 });
        }
    }
}
