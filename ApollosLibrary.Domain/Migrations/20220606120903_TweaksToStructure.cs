using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class TweaksToStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("16d6fda8-a9a0-4cb4-bd17-86bdd2286554"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SeriesRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Series",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "BusinessRecords",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Business",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuthorRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("f6f13bbe-78cb-458c-ae75-0aaa3e14223c"), new DateTime(2102, 6, 6, 22, 8, 59, 248, DateTimeKind.Local).AddTicks(3178), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 22, 8, 59, 248, DateTimeKind.Local).AddTicks(3211), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_BookId",
                table: "BookRecords",
                column: "BookId");

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
                name: "FK_BookRecords_Books_BookId",
                table: "BookRecords",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRecords_Books_BookId",
                table: "BookRecords");

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
                name: "IX_BookRecords_BookId",
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
                keyValue: new Guid("f6f13bbe-78cb-458c-ae75-0aaa3e14223c"));

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SeriesRecords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BookRecords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuthorRecords");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "BusinessRecords",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Business",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("16d6fda8-a9a0-4cb4-bd17-86bdd2286554"), new DateTime(2102, 6, 6, 20, 37, 51, 355, DateTimeKind.Local).AddTicks(6459), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 6, 20, 37, 51, 355, DateTimeKind.Local).AddTicks(6496), 1 });
        }
    }
}
