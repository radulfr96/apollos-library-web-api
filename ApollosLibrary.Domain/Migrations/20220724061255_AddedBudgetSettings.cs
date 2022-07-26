using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class AddedBudgetSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("24e79f69-c680-47d4-acd0-97dcfe82c3d3"));

            migrationBuilder.CreateTable(
                name: "UserBudgetSettings",
                columns: table => new
                {
                    UserBudgetSettingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    YearlyBudget = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBudgetSettings", x => x.UserBudgetSettingId);
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("52c1a824-a3fa-428f-a433-2290b15db9e0"), new NodaTime.LocalDateTime(2032, 7, 21, 6, 12, 50).PlusNanoseconds(848929100L), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new NodaTime.LocalDateTime(2022, 7, 24, 6, 12, 50).PlusNanoseconds(848929100L), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBudgetSettings");

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: new Guid("52c1a824-a3fa-428f-a433-2290b15db9e0"));

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("24e79f69-c680-47d4-acd0-97dcfe82c3d3"), new NodaTime.LocalDateTime(2032, 7, 3, 11, 9, 50).PlusNanoseconds(746660100L), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new NodaTime.LocalDateTime(2022, 7, 6, 11, 9, 50).PlusNanoseconds(746660100L), 1 });
        }
    }
}
