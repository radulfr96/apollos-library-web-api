using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AdjustedRecordsForModeration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("d2e55007-b499-4324-acad-70f399591167"));

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorRecords",
                columns: table => new
                {
                    AuthorRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportedVersion = table.Column<bool>(type: "bit", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRecords", x => x.AuthorRecordId);
                    table.ForeignKey(
                        name: "FK_AuthorRecords_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookRecords",
                columns: table => new
                {
                    BookRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportedVersion = table.Column<bool>(type: "bit", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EIsbn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<int>(type: "int", nullable: true),
                    PublicationFormatId = table.Column<int>(type: "int", nullable: false),
                    FictionTypeId = table.Column<int>(type: "int", nullable: false),
                    FormTypeId = table.Column<int>(type: "int", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: true),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRecords", x => x.BookRecordId);
                    table.ForeignKey(
                        name: "FK_BookRecords_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "BusinessId");
                    table.ForeignKey(
                        name: "FK_BookRecords_FictionTypes_FictionTypeId",
                        column: x => x.FictionTypeId,
                        principalTable: "FictionTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRecords_FormTypes_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "FormTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRecords_PublicationFormats_PublicationFormatId",
                        column: x => x.PublicationFormatId,
                        principalTable: "PublicationFormats",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRecords",
                columns: table => new
                {
                    BusinessRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportedVersion = table.Column<bool>(type: "bit", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusinessTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRecords", x => x.BusinessRecordId);
                    table.ForeignKey(
                        name: "FK_BusinessRecords_BusinessTypes_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalTable: "BusinessTypes",
                        principalColumn: "BusinessTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessRecords_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesRecords",
                columns: table => new
                {
                    SeriesRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportedVersion = table.Column<bool>(type: "bit", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesRecords", x => x.SeriesRecordId);
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("688ab592-2feb-4497-9477-51fc9be8379a"), new DateTime(2102, 6, 4, 22, 48, 23, 235, DateTimeKind.Local).AddTicks(9409), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 4, 22, 48, 23, 235, DateTimeKind.Local).AddTicks(9443), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRecords_CountryId",
                table: "AuthorRecords",
                column: "CountryId");

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

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRecords_BusinessTypeId",
                table: "BusinessRecords",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRecords_CountryId",
                table: "BusinessRecords",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorRecords");

            migrationBuilder.DropTable(
                name: "BookRecords");

            migrationBuilder.DropTable(
                name: "BusinessRecords");

            migrationBuilder.DropTable(
                name: "SeriesRecords");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("688ab592-2feb-4497-9477-51fc9be8379a"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Series",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Series",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Business",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Business",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Authors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Authors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("d2e55007-b499-4324-acad-70f399591167"), new DateTime(2102, 6, 4, 22, 34, 32, 368, DateTimeKind.Local).AddTicks(9479), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 6, 4, 22, 34, 32, 368, DateTimeKind.Local).AddTicks(9524), 1 });
        }
    }
}
