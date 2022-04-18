using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class FixedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Businesss_BusinessId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesss_BusinessType_BusinessTypeId",
                table: "Businesss");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesss_Countries_CountryId",
                table: "Businesss");

            migrationBuilder.DropTable(
                name: "BusinessType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Businesss",
                table: "Businesss");

            migrationBuilder.RenameTable(
                name: "Businesss",
                newName: "Business");

            migrationBuilder.RenameIndex(
                name: "IX_Businesss_CountryId",
                table: "Business",
                newName: "IX_Business_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Businesss_BusinessTypeId",
                table: "Business",
                newName: "IX_Business_BusinessTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Business",
                table: "Business",
                column: "BusinessId");

            migrationBuilder.CreateTable(
                name: "BusinessTypes",
                columns: table => new
                {
                    BusinessTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTypes", x => x.BusinessTypeId);
                });

            migrationBuilder.InsertData(
                table: "BusinessTypes",
                columns: new[] { "BusinessTypeId", "Name" },
                values: new object[] { 1, "Publisher" });

            migrationBuilder.InsertData(
                table: "BusinessTypes",
                columns: new[] { "BusinessTypeId", "Name" },
                values: new object[] { 2, "Bookshop" });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Business_BusinessId",
                table: "Books",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_BusinessTypes_BusinessTypeId",
                table: "Business",
                column: "BusinessTypeId",
                principalTable: "BusinessTypes",
                principalColumn: "BusinessTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Business_Countries_CountryId",
                table: "Business",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Business_BusinessId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Business_BusinessTypes_BusinessTypeId",
                table: "Business");

            migrationBuilder.DropForeignKey(
                name: "FK_Business_Countries_CountryId",
                table: "Business");

            migrationBuilder.DropTable(
                name: "BusinessTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Business",
                table: "Business");

            migrationBuilder.RenameTable(
                name: "Business",
                newName: "Businesss");

            migrationBuilder.RenameIndex(
                name: "IX_Business_CountryId",
                table: "Businesss",
                newName: "IX_Businesss_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Business_BusinessTypeId",
                table: "Businesss",
                newName: "IX_Businesss_BusinessTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Businesss",
                table: "Businesss",
                column: "BusinessId");

            migrationBuilder.CreateTable(
                name: "BusinessType",
                columns: table => new
                {
                    BusinessTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessType", x => x.BusinessTypeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Businesss_BusinessId",
                table: "Books",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesss_BusinessType_BusinessTypeId",
                table: "Businesss",
                column: "BusinessTypeId",
                principalTable: "BusinessType",
                principalColumn: "BusinessTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesss_Countries_CountryId",
                table: "Businesss",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }
    }
}
