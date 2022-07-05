using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessTypes",
                columns: table => new
                {
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTypes", x => x.BusinessTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "EntryReportStatus",
                columns: table => new
                {
                    EntryReportStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReportStatus", x => x.EntryReportStatusId);
                });

            migrationBuilder.CreateTable(
                name: "EntryReportType",
                columns: table => new
                {
                    EntryReportTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReportType", x => x.EntryReportTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ErrorCodes",
                columns: table => new
                {
                    ErrorCodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorCodes", x => x.ErrorCodeId);
                });

            migrationBuilder.CreateTable(
                name: "FictionTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FictionTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "FormTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    LibraryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.LibraryId);
                });

            migrationBuilder.CreateTable(
                name: "PublicationFormats",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationFormats", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    SubscriptionTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionName = table.Column<string>(type: "text", nullable: true),
                    MonthlyRate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    StripeProductId = table.Column<string>(type: "text", nullable: true),
                    Purchasable = table.Column<bool>(type: "boolean", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    MaxUsers = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTypes", x => x.SubscriptionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                    table.ForeignKey(
                        name: "FK_Authors_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    BusinessId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    StreetAddress = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Postcode = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.BusinessId);
                    table.ForeignKey(
                        name: "FK_Business_BusinessTypes_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalTable: "BusinessTypes",
                        principalColumn: "BusinessTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Business_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateTable(
                name: "EntryReports",
                columns: table => new
                {
                    EntryReportId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryRecordId = table.Column<int>(type: "integer", nullable: false),
                    EntryTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntryReportStatusId = table.Column<int>(type: "integer", nullable: false),
                    ReportedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReports", x => x.EntryReportId);
                    table.ForeignKey(
                        name: "FK_EntryReports_EntryReportStatus_EntryReportStatusId",
                        column: x => x.EntryReportStatusId,
                        principalTable: "EntryReportStatus",
                        principalColumn: "EntryReportStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryReports_EntryReportType_EntryTypeId",
                        column: x => x.EntryTypeId,
                        principalTable: "EntryReportType",
                        principalColumn: "EntryReportTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesRecords",
                columns: table => new
                {
                    SeriesRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportedVersion = table.Column<bool>(type: "boolean", nullable: false),
                    SeriesId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesRecords", x => x.SeriesRecordId);
                    table.ForeignKey(
                        name: "FK_SeriesRecords_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionTypeId = table.Column<int>(type: "integer", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SubscriptionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubscriptionAdmin = table.Column<Guid>(type: "uuid", nullable: false),
                    StripeSubscriptionId = table.Column<string>(type: "text", nullable: true),
                    StripeCustomerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "SubscriptionTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorRecords",
                columns: table => new
                {
                    AuthorRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportedVersion = table.Column<bool>(type: "boolean", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRecords", x => x.AuthorRecordId);
                    table.ForeignKey(
                        name: "FK_AuthorRecords_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorRecords_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionId = table.Column<int>(type: "integer", nullable: false),
                    Isbn = table.Column<string>(type: "text", nullable: true),
                    EIsbn = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Subtitle = table.Column<string>(type: "text", nullable: true),
                    Edition = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    PublicationFormatId = table.Column<int>(type: "integer", nullable: false),
                    FictionTypeId = table.Column<int>(type: "integer", nullable: false),
                    FormTypeId = table.Column<int>(type: "integer", nullable: false),
                    BusinessId = table.Column<int>(type: "integer", nullable: true),
                    CoverImage = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "BusinessId");
                    table.ForeignKey(
                        name: "FK_Books_FictionTypes_FictionTypeId",
                        column: x => x.FictionTypeId,
                        principalTable: "FictionTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_FormTypes_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "FormTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_PublicationFormats_PublicationFormatId",
                        column: x => x.PublicationFormatId,
                        principalTable: "PublicationFormats",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessRecords",
                columns: table => new
                {
                    BusinessRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportedVersion = table.Column<bool>(type: "boolean", nullable: false),
                    BusinessId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    StreetAddress = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Postcode = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRecords", x => x.BusinessRecordId);
                    table.ForeignKey(
                        name: "FK_BusinessRecords_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BusinessId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    UserSubscrptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptions", x => x.UserSubscrptionId);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsAuthorId = table.Column<int>(type: "integer", nullable: false),
                    BooksBookId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsAuthorId, x.BooksBookId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsAuthorId",
                        column: x => x.AuthorsAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                columns: table => new
                {
                    BooksBookId = table.Column<int>(type: "integer", nullable: false),
                    GenresGenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BooksBookId, x.GenresGenreId });
                    table.ForeignKey(
                        name: "FK_BookGenre_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookRecords",
                columns: table => new
                {
                    BookRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportedVersion = table.Column<bool>(type: "boolean", nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    Isbn = table.Column<string>(type: "text", nullable: true),
                    EIsbn = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Subtitle = table.Column<string>(type: "text", nullable: true),
                    Edition = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    PublicationFormatId = table.Column<int>(type: "integer", nullable: false),
                    FictionTypeId = table.Column<int>(type: "integer", nullable: false),
                    FormTypeId = table.Column<int>(type: "integer", nullable: false),
                    BusinessId = table.Column<int>(type: "integer", nullable: true),
                    CoverImage = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRecords", x => x.BookRecordId);
                    table.ForeignKey(
                        name: "FK_BookRecords_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookSeries",
                columns: table => new
                {
                    BooksBookId = table.Column<int>(type: "integer", nullable: false),
                    SeriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSeries", x => new { x.BooksBookId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_BookSeries_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryEntries",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    LibraryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryEntries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_LibraryEntries_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryEntries_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "LibraryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BusinessTypes",
                columns: new[] { "BusinessTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Publisher" },
                    { 2, "Bookshop" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
                    { "AD", "Andorra" },
                    { "AE", "United Arab Emirates" },
                    { "AF", "Afghanistan" },
                    { "AG", "Antigua and Barbuda" },
                    { "AI", "Anguilla" },
                    { "AL", "Albania" },
                    { "AM", "Armenia" },
                    { "AO", "Angola" },
                    { "AQ", "Antarctica" },
                    { "AR", "Argentina" },
                    { "AS", "American Samoa" },
                    { "AT", "Austria" },
                    { "AU", "Australia" },
                    { "AW", "Aruba" },
                    { "AX", "Åland Islands" },
                    { "AZ", "Azerbaijan" },
                    { "BA", "Bosnia and Herzegovina" },
                    { "BB", "Barbados" },
                    { "BD", "Bangladesh" },
                    { "BE", "Belgium" },
                    { "BF", "Burkina Faso" },
                    { "BG", "Bulgaria" },
                    { "BH", "Bahrain" },
                    { "BI", "Burundi" },
                    { "BJ", "Benin" },
                    { "BL", "Saint BarthÃ©lemy" },
                    { "BM", "Bermuda" },
                    { "BN", "Brunei Darussalam" },
                    { "BO", "Bolivia, Plurinational State of" },
                    { "BQ", "Bonaire, Sint Eustatius and Saba" },
                    { "BR", "Brazil" },
                    { "BS", "Bahamas" },
                    { "BT", "Bhutan" },
                    { "BV", "Bouvet Island" },
                    { "BW", "Botswana" },
                    { "BY", "Belarus" },
                    { "BZ", "Belize" },
                    { "CA", "Canada" },
                    { "CC", "Cocos (Keeling) Islands" },
                    { "CD", "Congo, the Democratic Republic of the" },
                    { "CF", "Central African Republic" },
                    { "CG", "Congo" },
                    { "CH", "Switzerland" },
                    { "CI", "CÃ´te d'Ivoire" },
                    { "CK", "Cook Islands" },
                    { "CL", "Chile" },
                    { "CM", "Cameroon" },
                    { "CN", "China" },
                    { "CO", "Colombia" },
                    { "CR", "Costa Rica" },
                    { "CU", "Cuba" },
                    { "CV", "Cape Verde" },
                    { "CW", "CuraÃ§ao" },
                    { "CX", "Christmas Island" },
                    { "CY", "Cyprus" },
                    { "CZ", "Czech Republic" },
                    { "DE", "Germany" },
                    { "DJ", "Djibouti" },
                    { "DK", "Denmark" },
                    { "DM", "Dominica" },
                    { "DO", "Dominican Republic" },
                    { "DZ", "Algeria" },
                    { "EC", "Ecuador" },
                    { "EE", "Estonia" },
                    { "EG", "Egypt" },
                    { "EH", "Western Sahara" },
                    { "ER", "Eritrea" },
                    { "ES", "Spain" },
                    { "ET", "Ethiopia" },
                    { "FI", "Finland" },
                    { "FJ", "Fiji" },
                    { "FK", "Falkland Islands (Malvinas)" },
                    { "FM", "Micronesia, Federated States of" },
                    { "FO", "Faroe Islands" },
                    { "FR", "France" },
                    { "GA", "Gabon" },
                    { "GB", "United Kingdom" },
                    { "GD", "Grenada" },
                    { "GE", "Georgia" },
                    { "GF", "French Guiana" },
                    { "GG", "Guernsey" },
                    { "GH", "Ghana" },
                    { "GI", "Gibraltar" },
                    { "GL", "Greenland" },
                    { "GM", "Gambia" },
                    { "GN", "Guinea" },
                    { "GP", "Guadeloupe" },
                    { "GQ", "Equatorial Guinea" },
                    { "GR", "Greece" },
                    { "GS", "South Georgia and the South Sandwich Islands" },
                    { "GT", "Guatemala" },
                    { "GU", "Guam" },
                    { "GW", "Guinea-Bissau" },
                    { "GY", "Guyana" },
                    { "HK", "Hong Kong" },
                    { "HM", "Heard Island and McDonald Islands" },
                    { "HN", "Honduras" },
                    { "HR", "Croatia" },
                    { "HT", "Haiti" },
                    { "HU", "Hungary" },
                    { "ID", "Indonesia" },
                    { "IE", "Ireland" },
                    { "IL", "Israel" },
                    { "IM", "Isle of Man" },
                    { "IN", "India" },
                    { "IO", "British Indian Ocean Territory" },
                    { "IQ", "Iraq" },
                    { "IR", "Iran, Islamic Republic of" },
                    { "IS", "Iceland" },
                    { "IT", "Italy" },
                    { "JE", "Jersey" },
                    { "JM", "Jamaica" },
                    { "JO", "Jordan" },
                    { "JP", "Japan" },
                    { "KE", "Kenya" },
                    { "KG", "Kyrgyzstan" },
                    { "KH", "Cambodia" },
                    { "KI", "Kiribati" },
                    { "KM", "Comoros" },
                    { "KN", "Saint Kitts and Nevis" },
                    { "KP", "Korea, Democratic People's Republic of" },
                    { "KR", "Korea, Republic of" },
                    { "KW", "Kuwait" },
                    { "KY", "Cayman Islands" },
                    { "KZ", "Kazakhstan" },
                    { "LA", "Lao People's Democratic Republic" },
                    { "LB", "Lebanon" },
                    { "LC", "Saint Lucia" },
                    { "LI", "Liechtenstein" },
                    { "LK", "Sri Lanka" },
                    { "LR", "Liberia" },
                    { "LS", "Lesotho" },
                    { "LT", "Lithuania" },
                    { "LU", "Luxembourg" },
                    { "LV", "Latvia" },
                    { "LY", "Libya" },
                    { "MA", "Morocco" },
                    { "MC", "Monaco" },
                    { "MD", "Moldova, Republic of" },
                    { "ME", "Montenegro" },
                    { "MF", "Saint Martin (French part)" },
                    { "MG", "Madagascar" },
                    { "MH", "Marshall Islands" },
                    { "MK", "Macedonia, the Former Yugoslav Republic of" },
                    { "ML", "Mali" },
                    { "MM", "Myanmar" },
                    { "MN", "Mongolia" },
                    { "MO", "Macao" },
                    { "MP", "Northern Mariana Islands" },
                    { "MQ", "Martinique" },
                    { "MR", "Mauritania" },
                    { "MS", "Montserrat" },
                    { "MT", "Malta" },
                    { "MU", "Mauritius" },
                    { "MV", "Maldives" },
                    { "MW", "Malawi" },
                    { "MX", "Mexico" },
                    { "MY", "Malaysia" },
                    { "MZ", "Mozambique" },
                    { "NA", "Namibia" },
                    { "NC", "New Caledonia" },
                    { "NE", "Niger" },
                    { "NF", "Norfolk Island" },
                    { "NG", "Nigeria" },
                    { "NI", "Nicaragua" },
                    { "NL", "Netherlands" },
                    { "NO", "Norway" },
                    { "NP", "Nepal" },
                    { "NR", "Nauru" },
                    { "NU", "Niue" },
                    { "NZ", "New Zealand" },
                    { "OM", "Oman" },
                    { "PA", "Panama" },
                    { "PE", "Peru" },
                    { "PF", "French Polynesia" },
                    { "PG", "Papua New Guinea" },
                    { "PH", "Philippines" },
                    { "PK", "Pakistan" },
                    { "PL", "Poland" },
                    { "PM", "Saint Pierre and Miquelon" },
                    { "PN", "Pitcairn" },
                    { "PR", "Puerto Rico" },
                    { "PS", "Palestine, State of" },
                    { "PT", "Portugal" },
                    { "PW", "Palau" },
                    { "PY", "Paraguay" },
                    { "QA", "Qatar" },
                    { "RE", "RÃ©union" },
                    { "RO", "Romania" },
                    { "RS", "Serbia" },
                    { "RU", "Russian Federation" },
                    { "RW", "Rwanda" },
                    { "SA", "Saudi Arabia" },
                    { "SB", "Solomon Islands" },
                    { "SC", "Seychelles" },
                    { "SD", "Sudan" },
                    { "SE", "Sweden" },
                    { "SG", "Singapore" },
                    { "SH", "Saint Helena, Ascension and Tristan da Cunha" },
                    { "SI", "Slovenia" },
                    { "SJ", "Svalbard and Jan Mayen" },
                    { "SK", "Slovakia" },
                    { "SL", "Sierra Leone" },
                    { "SM", "San Marino" },
                    { "SN", "Senegal" },
                    { "SO", "Somalia" },
                    { "SR", "Suriname" },
                    { "SS", "South Sudan" },
                    { "ST", "Sao Tome and Principe" },
                    { "SV", "El Salvador" },
                    { "SX", "Sint Maarten (Dutch part)" },
                    { "SY", "Syrian Arab Republic" },
                    { "SZ", "Swaziland" },
                    { "TC", "Turks and Caicos Islands" },
                    { "TD", "Chad" },
                    { "TF", "French Southern Territories" },
                    { "TG", "Togo" },
                    { "TH", "Thailand" },
                    { "TJ", "Tajikistan" },
                    { "TK", "Tokelau" },
                    { "TL", "Timor-Leste" },
                    { "TM", "Turkmenistan" },
                    { "TN", "Tunisia" },
                    { "TO", "Tonga" },
                    { "TR", "Turkey" },
                    { "TT", "Trinidad and Tobago" },
                    { "TV", "Tuvalu" },
                    { "TW", "Taiwan, Province of China" },
                    { "TZ", "Tanzania, United Republic of" },
                    { "UA", "Ukraine" },
                    { "UG", "Uganda" },
                    { "UM", "United States Minor Outlying Islands" },
                    { "US", "United States" },
                    { "UY", "Uruguay" },
                    { "UZ", "Uzbekistan" },
                    { "VA", "Holy See (Vatican City State)" },
                    { "VC", "Saint Vincent and the Grenadines" },
                    { "VE", "Venezuela, Bolivarian Republic of" },
                    { "VG", "Virgin Islands, British" },
                    { "VI", "Virgin Islands, U.S." },
                    { "VN", "Viet Nam" },
                    { "VU", "Vanuatu" },
                    { "WF", "Wallis and Futuna" },
                    { "WS", "Samoa" },
                    { "YE", "Yemen" },
                    { "YT", "Mayotte" },
                    { "ZA", "South Africa" },
                    { "ZM", "Zambia" },
                    { "ZW", "Zimbabwe" }
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
                table: "FictionTypes",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Non-Fiction" },
                    { 2, "Fiction" }
                });

            migrationBuilder.InsertData(
                table: "FormTypes",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Novel" },
                    { 2, "Novella" },
                    { 3, "Screenplay" },
                    { 4, "Manuscript" },
                    { 5, "Poem" },
                    { 6, "Textbook" }
                });

            migrationBuilder.InsertData(
                table: "PublicationFormats",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Printed" },
                    { 2, "eBook" },
                    { 3, "Audio Book" }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "SubscriptionTypeId", "Description", "IsAvailable", "MaxUsers", "MonthlyRate", "Purchasable", "StripeProductId", "SubscriptionName" },
                values: new object[,]
                {
                    { -1, null, true, 1, 0.00m, false, null, "Signed Up" },
                    { 1, null, true, 1, 0.00m, false, null, "Staff Member" },
                    { 2, "This subscription is for individuals keeping track of their own library.", true, 1, 10.00m, true, "prod_LlBGpg7ytim1dy", "Individual Subscription" },
                    { 3, "This subscription is for families keeping track of their own libraries. Each user will have their own library.", false, 5, 30.00m, true, "prod_LlBHeWO1QAe9Dx", "Family Subscription" }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ExpiryDate", "StripeCustomerId", "StripeSubscriptionId", "SubscriptionAdmin", "SubscriptionDate", "SubscriptionTypeId" },
                values: new object[] { new Guid("78c18a1d-e5c5-4821-a13f-0bd18a9b8c2d"), new DateTime(2102, 7, 5, 22, 38, 28, 155, DateTimeKind.Local).AddTicks(7614), null, null, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2022, 7, 5, 22, 38, 28, 155, DateTimeKind.Local).AddTicks(7651), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksBookId",
                table: "AuthorBook",
                column: "BooksBookId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRecords_AuthorId",
                table: "AuthorRecords",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRecords_CountryId",
                table: "AuthorRecords",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CountryId",
                table: "Authors",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenresGenreId",
                table: "BookGenre",
                column: "GenresGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_BookId",
                table: "BookRecords",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BusinessId",
                table: "Books",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_FictionTypeId",
                table: "Books",
                column: "FictionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_FormTypeId",
                table: "Books",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublicationFormatId",
                table: "Books",
                column: "PublicationFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSeries_SeriesId",
                table: "BookSeries",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessTypeId",
                table: "Business",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_CountryId",
                table: "Business",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRecords_BusinessId",
                table: "BusinessRecords",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_EntryReportStatusId",
                table: "EntryReports",
                column: "EntryReportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_EntryTypeId",
                table: "EntryReports",
                column: "EntryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryEntries_BookId",
                table: "LibraryEntries",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryEntries_LibraryId",
                table: "LibraryEntries",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BusinessId",
                table: "Orders",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesRecords_SeriesId",
                table: "SeriesRecords",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriptionTypeId",
                table: "Subscriptions",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_SubscriptionId",
                table: "UserSubscriptions",
                column: "SubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "AuthorRecords");

            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.DropTable(
                name: "BookRecords");

            migrationBuilder.DropTable(
                name: "BookSeries");

            migrationBuilder.DropTable(
                name: "BusinessRecords");

            migrationBuilder.DropTable(
                name: "EntryReports");

            migrationBuilder.DropTable(
                name: "ErrorCodes");

            migrationBuilder.DropTable(
                name: "LibraryEntries");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "SeriesRecords");

            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "EntryReportStatus");

            migrationBuilder.DropTable(
                name: "EntryReportType");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "FictionTypes");

            migrationBuilder.DropTable(
                name: "FormTypes");

            migrationBuilder.DropTable(
                name: "PublicationFormats");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");

            migrationBuilder.DropTable(
                name: "BusinessTypes");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
