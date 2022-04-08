using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class UpdateToIncludeSeriesOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Series_SeriesId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_SeriesId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "NumberInSeries",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Series",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Series",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.CreateTable(
                name: "SeriesOrder",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesOrder", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_SeriesOrder_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesOrder_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeriesOrder_BookId",
                table: "SeriesOrder",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesOrder_SeriesId",
                table: "SeriesOrder",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeriesOrder");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Series");

            migrationBuilder.AddColumn<decimal>(
                name: "NumberInSeries",
                table: "Books",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_SeriesId",
                table: "Books",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Series_SeriesId",
                table: "Books",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "SeriesId");
        }
    }
}
