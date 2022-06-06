using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class UpdatedEntityRefs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecords_Business_BusinessId",
                table: "BookRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRecords_FictionTypes_FictionTypeId",
                table: "BookRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRecords_FormTypes_FormTypeId",
                table: "BookRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRecords_PublicationFormats_PublicationFormatId",
                table: "BookRecords");

            migrationBuilder.DropIndex(
                name: "IX_BookRecords_BusinessId",
                table: "BookRecords");

            migrationBuilder.DropIndex(
                name: "IX_BookRecords_FictionTypeId",
                table: "BookRecords");

            migrationBuilder.DropIndex(
                name: "IX_BookRecords_FormTypeId",
                table: "BookRecords");

            migrationBuilder.DropIndex(
                name: "IX_BookRecords_PublicationFormatId",
                table: "BookRecords");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("481fddbf-0c92-460d-b2cf-548af39bc7ce"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("e960f3d9-444d-4142-8eb1-f35e3d2649fb"), new DateTime(2102, 6, 6, 20, 21, 13, 45, DateTimeKind.Local).AddTicks(1142), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 21, 13, 45, DateTimeKind.Local).AddTicks(1178), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("e960f3d9-444d-4142-8eb1-f35e3d2649fb"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("481fddbf-0c92-460d-b2cf-548af39bc7ce"), new DateTime(2102, 6, 6, 20, 15, 42, 377, DateTimeKind.Local).AddTicks(9364), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 15, 42, 377, DateTimeKind.Local).AddTicks(9400), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_BusinessId",
                table: "BookRecords",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_FictionTypeId",
                table: "BookRecords",
                column: "FictionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_FormTypeId",
                table: "BookRecords",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_PublicationFormatId",
                table: "BookRecords",
                column: "PublicationFormatId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecords_Business_BusinessId",
                table: "BookRecords",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecords_FictionTypes_FictionTypeId",
                table: "BookRecords",
                column: "FictionTypeId",
                principalTable: "FictionTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecords_FormTypes_FormTypeId",
                table: "BookRecords",
                column: "FormTypeId",
                principalTable: "FormTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRecords_PublicationFormats_PublicationFormatId",
                table: "BookRecords",
                column: "PublicationFormatId",
                principalTable: "PublicationFormats",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
