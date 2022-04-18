using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApollosLibrary.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "ErrorCodes",
                columns: table => new
                {
                    ErrorCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorCodes", x => x.ErrorCodeId);
                });

            migrationBuilder.CreateTable(
                name: "FictionTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FictionTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "FormTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "PublicationFormats",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationFormats", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                name: "Businesss",
                columns: table => new
                {
                    BusinessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BusinessTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesss", x => x.BusinessId);
                    table.ForeignKey(
                        name: "FK_Businesss_BusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalTable: "BusinessType",
                        principalColumn: "BusinessTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Businesss_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EIsbn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edition = table.Column<int>(type: "int", nullable: true),
                    PublicationFormatId = table.Column<int>(type: "int", nullable: false),
                    FictionTypeId = table.Column<int>(type: "int", nullable: false),
                    FormTypeId = table.Column<int>(type: "int", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: true),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Businesss_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesss",
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
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsAuthorId = table.Column<int>(type: "int", nullable: false),
                    BooksBookId = table.Column<int>(type: "int", nullable: false)
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
                    BooksBookId = table.Column<int>(type: "int", nullable: false),
                    GenresGenreId = table.Column<int>(type: "int", nullable: false)
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
                name: "BookSeries",
                columns: table => new
                {
                    BooksBookId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false)
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
                    { "CG", "Congo" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
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
                    { "GL", "Greenland" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
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
                    { "LA", "Lao People's Democratic Republic" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
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
                    { "NP", "Nepal" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
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
                    { "SV", "El Salvador" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
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
                values: new object[] { 1, "Novel" });

            migrationBuilder.InsertData(
                table: "FormTypes",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
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

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksBookId",
                table: "AuthorBook",
                column: "BooksBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CountryId",
                table: "Authors",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenresGenreId",
                table: "BookGenre",
                column: "GenresGenreId");

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
                name: "IX_Businesss_BusinessTypeId",
                table: "Businesss",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesss_CountryId",
                table: "Businesss",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.DropTable(
                name: "BookSeries");

            migrationBuilder.DropTable(
                name: "ErrorCodes");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Businesss");

            migrationBuilder.DropTable(
                name: "FictionTypes");

            migrationBuilder.DropTable(
                name: "FormTypes");

            migrationBuilder.DropTable(
                name: "PublicationFormats");

            migrationBuilder.DropTable(
                name: "BusinessType");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
