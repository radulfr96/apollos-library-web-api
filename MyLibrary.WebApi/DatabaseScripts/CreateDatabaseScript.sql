USE [master]

USE master;
ALTER DATABASE MyLibrary SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE MyLibrary ;

CREATE DATABASE MyLibrary


USE MyLibrary
GO

CREATE SCHEMA [Users]
GO


CREATE SCHEMA [Genre]
GO


CREATE SCHEMA [Book]
GO


CREATE SCHEMA [Publisher]
GO

CREATE SCHEMA [Author]
GO


CREATE TABLE [Users].[User](
	[UserID] [uniqueidentifier] NOT NULL,
	[Subject] [varchar](200) NOT NULL,
	[Username] [varchar](200) NULL,
	[Password] [text] NULL,
	[IsActive] [bit] NOT NULL,
	[SecurityCode] [varchar] (200) NULL,
	[SecurityCodeExpirationDate] [datetime] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [UNIQUEIDENTIFIER] NULL,
	CONSTRAINT [PK_User] PRIMARY KEY (UserID)
	)


CREATE TABLE [Users].[UserClaim](
	[ID] [uniqueidentifier] NOT NULL,
	[Type] [VARCHAR](250) NOT NULL,
	[Value] [varchar](250) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_UserClaim] PRIMARY KEY (ID),
	CONSTRAINT [FK_UserUserClaim] FOREIGN KEY ([UserID]) REFERENCES [Users].[User](UserID)
	)


CREATE TABLE [Genre].[Genre]
(
	[GenreID] INT IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedDate] DATETIME,
	[ModifiedBy] UNIQUEIDENTIFIER,
	CONSTRAINT PK_Genre PRIMARY KEY (GenreID),
)


CREATE TABLE [Book].[Series]
(
	[SeriesID] INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(200)
)

CREATE TABLE [Book].[PublicationFormat]
(
	TypeID INT IDENTITY NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	CONSTRAINT PK_PublicationFormat PRIMARY KEY (TypeID)
)

CREATE TABLE [Book].[FictionType]
(
	[TypeID] INT IDENTITY NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	CONSTRAINT PK_FictionType PRIMARY KEY (TypeID)
)

CREATE TABLE [Book].[FormType]
(
	[TypeID] INT IDENTITY NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	CONSTRAINT PK_FormType PRIMARY KEY (TypeID)
)

CREATE TABLE [dbo].[ErrorCode]
(
	[ErrorCodeId] INT NOT NULL PRIMARY KEY,
	[Message] VARCHAR(80) NOT NULL,
)

CREATE TABLE [dbo].[Country]
(
	[CountryID] CHAR(2) NOT NULL PRIMARY KEY,
	[Name] VARCHAR(80) NOT NULL,
)

CREATE TABLE [Publisher].[Publisher]
(
	[PublisherID] INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(200) NOT NULL,
	[Website] VARCHAR(200),
	[Street Address] VARCHAR(100) NOT NULL,
	[City] VARCHAR(100) NOT NULL,
	[Postcode] VARCHAR(20) NOT NULL,
	[State] VARCHAR(50) NOT NULL,
	[CountryID] CHAR(2) NOT NULL,
	[IsDeleted] BIT,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedDate] DATETIME,
	[ModifiedBy] UNIQUEIDENTIFIER,
	CONSTRAINT FK_PublisherCountry
	FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] (CountryID),
)


CREATE TABLE [Author].[Author]
(
	[AuthorID] INT PRIMARY KEY IDENTITY NOT NULL,
	[FirstName] VARCHAR(50) NOT NULL,
	[MiddleName] VARCHAR(50),
	[LastName] VARCHAR(50),
	[Description] VARCHAR(2000),
	[CountryID] CHAR(2) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] UNIQUEIDENTIFIER NOT NULL,
	[ModifiedDate] DATETIME,
	[ModifiedBy] UNIQUEIDENTIFIER,
	CONSTRAINT FK_AuthorCountry
	FOREIGN KEY (CountryID) REFERENCES [dbo].[Country] (CountryID) 
)

CREATE TABLE [Book].[Book]
(
	[BookID] INT IDENTITY NOT NULL,
	[ISBN] VARCHAR(13),
	[eISBN] VARCHAR(13),
	[Title] VARCHAR(200) NOT NULL,
	[Subtitle] VARCHAR(200),
	[SeriesID] INT,
	[NumberInSeries] DECIMAL,
	[Edition] INT,
	[PublicationFormatID] INT NOT NULL,
	[FictionTypeID] INT NOT NULL,
	[FormTypeID] INT NOT NULL,
	[PublisherID] INT NOT NULL,
	[CoverImage] TEXT,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] UNIQUEIDENTIFIER NOT NULL,
	[ModifiedDate] DATETIME,
	[ModifiedBy] UNIQUEIDENTIFIER,
	CONSTRAINT PK_Book PRIMARY KEY (BookID),
	CONSTRAINT FK_SeriesBook
	FOREIGN KEY ([SeriesID])
	REFERENCES [Book].[Series] (SeriesID),
	CONSTRAINT FK_PublicationFormatBook
	FOREIGN KEY ([PublicationFormatID]) 
	REFERENCES [Book].[PublicationFormat] (TypeID),
	CONSTRAINT FK_FictionTypeBook
	FOREIGN KEY ([FictionTypeID])
	REFERENCES [Book].[FictionType] (TypeID),
	CONSTRAINT FK_FormTypeBook
	FOREIGN KEY ([FormTypeID]) 
	REFERENCES [Book].[FormType] (TypeID),
	CONSTRAINT FK_BookPublisher
	FOREIGN KEY (PublisherID)
	REFERENCES Publisher.Publisher (PublisherID),
)

CREATE TABLE [Book].[BookGenre]
(
	[GenreID] INT NOT NULL,
	BookID INT NOT NULL,
	CONSTRAINT PK_BookGenre PRIMARY KEY (GenreID, BookID),
	CONSTRAINT FK_BookGenreGenre FOREIGN KEY (GenreID)
	REFERENCES [Genre].[Genre] (GenreID),
	CONSTRAINT FK_BookGenreBook FOREIGN KEY (BookID)
	REFERENCES [Book].[Book] (BookID)
)

CREATE TABLE [Book].[BookAuthor]
(
	[AuthorID] INT NOT NULL,
	BookID INT NOT NULL,
	CONSTRAINT PK_BookAuthor PRIMARY KEY (AuthorID, BookID),
	CONSTRAINT FK_BookAuthorAuthor FOREIGN KEY (AuthorID)
	REFERENCES [Author].[Author] (AuthorID),
	CONSTRAINT FK_BookAuthorBook FOREIGN KEY (BookID)
	REFERENCES [Book].[Book] (BookID)
)

DECLARE @UserID UNIQUEIDENTIFIER


INSERT INTO [Users].[User] ([UserID], [Subject], [Username], [Password], [CreatedDate], [CreatedBy], [IsActive])
VALUES (NEWID(), NEWID(), 'radulfr', 'AQAAAAEAACcQAAAAECY64tCZ5CSbcXzOp4NE6XAr1TB9wQ1zgMv6Sa49QGTmEftnFXzPMsBH+NB1cu5brw==', GETDATE(), NEWID(), 1)

SELECT @UserID = U.UserID
FROM Users.[User] U
WHERE U.Username = 'radulfr'

INSERT INTO [Users].UserClaim (ID, UserID, Type, Value)
VALUES (NEWID(), @UserID, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'administrator'),
(NEWID(), @UserID, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'moderator'),
(NEWID(), @UserID, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 'freeaccount'),
(NEWID(), @UserID, 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', 'wados.russell70@gmail.com')


UPDATE U
SET U.CreatedBy = U.UserID
FROM Users.[User] U
WHERE U.Username = 'radulfr'

INSERT INTO [Book].PublicationFormat ([Name])
VALUES ('Printed'), ('eBook'), ('Audio Book')

INSERT INTO [Book].FictionType ([Name])
VALUES ('Fiction'), ('Non-fiction')

INSERT INTO [Book].FormType ([Name])
VALUES ('Novel'), ('Novella'), ('Screenplay'), ('Manuscript'), ('Poem'), ('Text Book')

INSERT INTO [dbo].[Country] VALUES ('AD','Andorra')
INSERT INTO [dbo].[Country] VALUES ('AE','United Arab Emirates')
INSERT INTO [dbo].[Country] VALUES ('AF','Afghanistan')
INSERT INTO [dbo].[Country] VALUES ('AG','Antigua and Barbuda')
INSERT INTO [dbo].[Country] VALUES ('AI','Anguilla')
INSERT INTO [dbo].[Country] VALUES ('AL','Albania')
INSERT INTO [dbo].[Country] VALUES ('AM','Armenia')
INSERT INTO [dbo].[Country] VALUES ('AN','Netherlands Antilles')
INSERT INTO [dbo].[Country] VALUES ('AO','Anla')
INSERT INTO [dbo].[Country] VALUES ('AQ','Antarctica')
INSERT INTO [dbo].[Country] VALUES ('AR','Argentina')
INSERT INTO [dbo].[Country] VALUES ('AT','Austria')
INSERT INTO [dbo].[Country] VALUES ('AU','Australia')
INSERT INTO [dbo].[Country] VALUES ('AW','Aruba')
INSERT INTO [dbo].[Country] VALUES ('AZ','Azerbaijan')
INSERT INTO [dbo].[Country] VALUES ('BA','Bosnia and Herzevina')
INSERT INTO [dbo].[Country] VALUES ('BB','Barbados')
INSERT INTO [dbo].[Country] VALUES ('BD','Bangladesh')
INSERT INTO [dbo].[Country] VALUES ('BE','Belgium')
INSERT INTO [dbo].[Country] VALUES ('BF','Burkina Faso')
INSERT INTO [dbo].[Country] VALUES ('BG','Bulgaria')
INSERT INTO [dbo].[Country] VALUES ('BH','Bahrain')
INSERT INTO [dbo].[Country] VALUES ('BI','Burundi')
INSERT INTO [dbo].[Country] VALUES ('BJ','Benin')
INSERT INTO [dbo].[Country] VALUES ('BM','Bermuda')
INSERT INTO [dbo].[Country] VALUES ('BN','Brunei Darussalam')
INSERT INTO [dbo].[Country] VALUES ('BO','Bolivia')
INSERT INTO [dbo].[Country] VALUES ('BR','Brazil')
INSERT INTO [dbo].[Country] VALUES ('BS','Bahamas')
INSERT INTO [dbo].[Country] VALUES ('BT','Bhutan')
INSERT INTO [dbo].[Country] VALUES ('BV','Bouvet Island')
INSERT INTO [dbo].[Country] VALUES ('BW','Botswana')
INSERT INTO [dbo].[Country] VALUES ('BY','Belarus')
INSERT INTO [dbo].[Country] VALUES ('BZ','Belize')
INSERT INTO [dbo].[Country] VALUES ('CA','Canada')
INSERT INTO [dbo].[Country] VALUES ('CC','Cocos (Keeling) Islands')
INSERT INTO [dbo].[Country] VALUES ('CF','Central African Republic')
INSERT INTO [dbo].[Country] VALUES ('CG','Con')
INSERT INTO [dbo].[Country] VALUES ('CH','Switzerland')
INSERT INTO [dbo].[Country] VALUES ('CI','Ivory Coast')
INSERT INTO [dbo].[Country] VALUES ('CK','Cook Islands')
INSERT INTO [dbo].[Country] VALUES ('CL','Chile')
INSERT INTO [dbo].[Country] VALUES ('CM','Cameroon')
INSERT INTO [dbo].[Country] VALUES ('CN','China')
INSERT INTO [dbo].[Country] VALUES ('CO','Colombia')
INSERT INTO [dbo].[Country] VALUES ('CR','Costa Rica')
INSERT INTO [dbo].[Country] VALUES ('CU','Cuba')
INSERT INTO [dbo].[Country] VALUES ('CV','Cape Verde')
INSERT INTO [dbo].[Country] VALUES ('CX','Christmas Island')
INSERT INTO [dbo].[Country] VALUES ('CY','Cyprus')
INSERT INTO [dbo].[Country] VALUES ('CZ','Czech Republic')
INSERT INTO [dbo].[Country] VALUES ('DE','Germany')
INSERT INTO [dbo].[Country] VALUES ('DJ','Djibouti')
INSERT INTO [dbo].[Country] VALUES ('DK','Denmark')
INSERT INTO [dbo].[Country] VALUES ('DM','Dominica')
INSERT INTO [dbo].[Country] VALUES ('DO','Dominican Republic')
INSERT INTO [dbo].[Country] VALUES ('DS','American Samoa')
INSERT INTO [dbo].[Country] VALUES ('DZ','Algeria')
INSERT INTO [dbo].[Country] VALUES ('EC','Ecuador')
INSERT INTO [dbo].[Country] VALUES ('EE','Estonia')
INSERT INTO [dbo].[Country] VALUES ('EG','Egypt')
INSERT INTO [dbo].[Country] VALUES ('EH','Western Sahara')
INSERT INTO [dbo].[Country] VALUES ('ER','Eritrea')
INSERT INTO [dbo].[Country] VALUES ('ES','Spain')
INSERT INTO [dbo].[Country] VALUES ('ET','Ethiopia')
INSERT INTO [dbo].[Country] VALUES ('FI','Finland')
INSERT INTO [dbo].[Country] VALUES ('FJ','Fiji')
INSERT INTO [dbo].[Country] VALUES ('FK','Falkland Islands (Malvinas)')
INSERT INTO [dbo].[Country] VALUES ('FM','Micronesia, Federated States of')
INSERT INTO [dbo].[Country] VALUES ('FO','Faroe Islands')
INSERT INTO [dbo].[Country] VALUES ('FR','France')
INSERT INTO [dbo].[Country] VALUES ('FX','France, Metropolitan')
INSERT INTO [dbo].[Country] VALUES ('GA','Gabon')
INSERT INTO [dbo].[Country] VALUES ('GB','United Kingdom')
INSERT INTO [dbo].[Country] VALUES ('GD','Grenada')
INSERT INTO [dbo].[Country] VALUES ('GE','Georgia')
INSERT INTO [dbo].[Country] VALUES ('GF','French Guiana')
INSERT INTO [dbo].[Country] VALUES ('GH','Ghana')
INSERT INTO [dbo].[Country] VALUES ('GI','Gibraltar')
INSERT INTO [dbo].[Country] VALUES ('GK','Guernsey')
INSERT INTO [dbo].[Country] VALUES ('GL','Greenland')
INSERT INTO [dbo].[Country] VALUES ('GM','Gambia')
INSERT INTO [dbo].[Country] VALUES ('GN','Guinea')
INSERT INTO [dbo].[Country] VALUES ('GP','Guadeloupe')
INSERT INTO [dbo].[Country] VALUES ('GQ','Equatorial Guinea')
INSERT INTO [dbo].[Country] VALUES ('GR','Greece')
INSERT INTO [dbo].[Country] VALUES ('GS','South Georgia South Sandwich Islands')
INSERT INTO [dbo].[Country] VALUES ('GT','Guatemala')
INSERT INTO [dbo].[Country] VALUES ('GU','Guam')
INSERT INTO [dbo].[Country] VALUES ('GW','Guinea-Bissau')
INSERT INTO [dbo].[Country] VALUES ('GY','Guyana')
INSERT INTO [dbo].[Country] VALUES ('HK','Hong Kong')
INSERT INTO [dbo].[Country] VALUES ('HM','Heard and Mc Donald Islands')
INSERT INTO [dbo].[Country] VALUES ('HN','Honduras')
INSERT INTO [dbo].[Country] VALUES ('HR','Croatia (Hrvatska)')
INSERT INTO [dbo].[Country] VALUES ('HT','Haiti')
INSERT INTO [dbo].[Country] VALUES ('HU','Hungary')
INSERT INTO [dbo].[Country] VALUES ('ID','Indonesia')
INSERT INTO [dbo].[Country] VALUES ('IE','Ireland')
INSERT INTO [dbo].[Country] VALUES ('IL','Israel')
INSERT INTO [dbo].[Country] VALUES ('IM','Isle of Man')
INSERT INTO [dbo].[Country] VALUES ('IN','India')
INSERT INTO [dbo].[Country] VALUES ('IO','British Indian Ocean Territory')
INSERT INTO [dbo].[Country] VALUES ('IQ','Iraq')
INSERT INTO [dbo].[Country] VALUES ('IR','Iran (Islamic Republic of)')
INSERT INTO [dbo].[Country] VALUES ('IS','Iceland')
INSERT INTO [dbo].[Country] VALUES ('IT','Italy')
INSERT INTO [dbo].[Country] VALUES ('JE','Jersey')
INSERT INTO [dbo].[Country] VALUES ('JM','Jamaica')
INSERT INTO [dbo].[Country] VALUES ('JO','Jordan')
INSERT INTO [dbo].[Country] VALUES ('JP','Japan')
INSERT INTO [dbo].[Country] VALUES ('KE','Kenya')
INSERT INTO [dbo].[Country] VALUES ('KG','Kyrgyzstan')
INSERT INTO [dbo].[Country] VALUES ('KH','Cambodia')
INSERT INTO [dbo].[Country] VALUES ('KI','Kiribati')
INSERT INTO [dbo].[Country] VALUES ('KM','Comoros')
INSERT INTO [dbo].[Country] VALUES ('KN','Saint Kitts and Nevis')
INSERT INTO [dbo].[Country] VALUES ('KP','Korea, Democratic People''s Republic of')
INSERT INTO [dbo].[Country] VALUES ('KR','Korea, Republic of')
INSERT INTO [dbo].[Country] VALUES ('KW','Kuwait')
INSERT INTO [dbo].[Country] VALUES ('KY','Cayman Islands')
INSERT INTO [dbo].[Country] VALUES ('KZ','Kazakhstan')
INSERT INTO [dbo].[Country] VALUES ('LA','Lao People''s Democratic Republic')
INSERT INTO [dbo].[Country] VALUES ('LB','Lebanon')
INSERT INTO [dbo].[Country] VALUES ('LC','Saint Lucia')
INSERT INTO [dbo].[Country] VALUES ('LI','Liechtenstein')
INSERT INTO [dbo].[Country] VALUES ('LK','Sri Lanka')
INSERT INTO [dbo].[Country] VALUES ('LR','Liberia')
INSERT INTO [dbo].[Country] VALUES ('LS','Lesotho')
INSERT INTO [dbo].[Country] VALUES ('LT','Lithuania')
INSERT INTO [dbo].[Country] VALUES ('LU','Luxembourg')
INSERT INTO [dbo].[Country] VALUES ('LV','Latvia')
INSERT INTO [dbo].[Country] VALUES ('LY','Libyan Arab Jamahiriya')
INSERT INTO [dbo].[Country] VALUES ('MA','Morocco')
INSERT INTO [dbo].[Country] VALUES ('MC','Monaco')
INSERT INTO [dbo].[Country] VALUES ('MD','Moldova, Republic of')
INSERT INTO [dbo].[Country] VALUES ('ME','Montenegro')
INSERT INTO [dbo].[Country] VALUES ('MG','Madagascar')
INSERT INTO [dbo].[Country] VALUES ('MH','Marshall Islands')
INSERT INTO [dbo].[Country] VALUES ('MK','Macedonia')
INSERT INTO [dbo].[Country] VALUES ('ML','Mali')
INSERT INTO [dbo].[Country] VALUES ('MM','Myanmar')
INSERT INTO [dbo].[Country] VALUES ('MN','Monlia')
INSERT INTO [dbo].[Country] VALUES ('MO','Macau')
INSERT INTO [dbo].[Country] VALUES ('MP','Northern Mariana Islands')
INSERT INTO [dbo].[Country] VALUES ('MQ','Martinique')
INSERT INTO [dbo].[Country] VALUES ('MR','Mauritania')
INSERT INTO [dbo].[Country] VALUES ('MS','Montserrat')
INSERT INTO [dbo].[Country] VALUES ('MT','Malta')
INSERT INTO [dbo].[Country] VALUES ('MU','Mauritius')
INSERT INTO [dbo].[Country] VALUES ('MV','Maldives')
INSERT INTO [dbo].[Country] VALUES ('MW','Malawi')
INSERT INTO [dbo].[Country] VALUES ('MX','Mexico')
INSERT INTO [dbo].[Country] VALUES ('MY','Malaysia')
INSERT INTO [dbo].[Country] VALUES ('MZ','Mozambique')
INSERT INTO [dbo].[Country] VALUES ('NA','Namibia')
INSERT INTO [dbo].[Country] VALUES ('NC','New Caledonia')
INSERT INTO [dbo].[Country] VALUES ('NE','Niger')
INSERT INTO [dbo].[Country] VALUES ('NF','Norfolk Island')
INSERT INTO [dbo].[Country] VALUES ('NG','Nigeria')
INSERT INTO [dbo].[Country] VALUES ('NI','Nicaragua')
INSERT INTO [dbo].[Country] VALUES ('NL','Netherlands')
INSERT INTO [dbo].[Country] VALUES ('NO','Norway')
INSERT INTO [dbo].[Country] VALUES ('NP','Nepal')
INSERT INTO [dbo].[Country] VALUES ('NR','Nauru')
INSERT INTO [dbo].[Country] VALUES ('NU','Niue')
INSERT INTO [dbo].[Country] VALUES ('NZ','New Zealand')
INSERT INTO [dbo].[Country] VALUES ('OM','Oman')
INSERT INTO [dbo].[Country] VALUES ('PA','Panama')
INSERT INTO [dbo].[Country] VALUES ('PE','Peru')
INSERT INTO [dbo].[Country] VALUES ('PF','French Polynesia')
INSERT INTO [dbo].[Country] VALUES ('PG','Papua New Guinea')
INSERT INTO [dbo].[Country] VALUES ('PH','Philippines')
INSERT INTO [dbo].[Country] VALUES ('PK','Pakistan')
INSERT INTO [dbo].[Country] VALUES ('PL','Poland')
INSERT INTO [dbo].[Country] VALUES ('PM','St. Pierre and Miquelon')
INSERT INTO [dbo].[Country] VALUES ('PN','Pitcairn')
INSERT INTO [dbo].[Country] VALUES ('PR','Puerto Rico')
INSERT INTO [dbo].[Country] VALUES ('PS','Palestine')
INSERT INTO [dbo].[Country] VALUES ('PT','Portugal')
INSERT INTO [dbo].[Country] VALUES ('PW','Palau')
INSERT INTO [dbo].[Country] VALUES ('PY','Paraguay')
INSERT INTO [dbo].[Country] VALUES ('QA','Qatar')
INSERT INTO [dbo].[Country] VALUES ('RE','Reunion')
INSERT INTO [dbo].[Country] VALUES ('RO','Romania')
INSERT INTO [dbo].[Country] VALUES ('RS','Serbia')
INSERT INTO [dbo].[Country] VALUES ('RU','Russian Federation')
INSERT INTO [dbo].[Country] VALUES ('RW','Rwanda')
INSERT INTO [dbo].[Country] VALUES ('SA','Saudi Arabia')
INSERT INTO [dbo].[Country] VALUES ('SB','Solomon Islands')
INSERT INTO [dbo].[Country] VALUES ('SC','Seychelles')
INSERT INTO [dbo].[Country] VALUES ('SD','Sudan')
INSERT INTO [dbo].[Country] VALUES ('SE','Sweden')
INSERT INTO [dbo].[Country] VALUES ('SG','Singapore')
INSERT INTO [dbo].[Country] VALUES ('SH','St. Helena')
INSERT INTO [dbo].[Country] VALUES ('SI','Slovenia')
INSERT INTO [dbo].[Country] VALUES ('SJ','Svalbard and Jan Mayen Islands')
INSERT INTO [dbo].[Country] VALUES ('SK','Slovakia')
INSERT INTO [dbo].[Country] VALUES ('SL','Sierra Leone')
INSERT INTO [dbo].[Country] VALUES ('SM','San Marino')
INSERT INTO [dbo].[Country] VALUES ('SN','Senegal')
INSERT INTO [dbo].[Country] VALUES ('SO','Somalia')
INSERT INTO [dbo].[Country] VALUES ('SR','Suriname')
INSERT INTO [dbo].[Country] VALUES ('ST','Sao Tome and Principe')
INSERT INTO [dbo].[Country] VALUES ('SV','El Salvador')
INSERT INTO [dbo].[Country] VALUES ('SY','Syrian Arab Republic')
INSERT INTO [dbo].[Country] VALUES ('SZ','Swaziland')
INSERT INTO [dbo].[Country] VALUES ('TC','Turks and Caicos Islands')
INSERT INTO [dbo].[Country] VALUES ('TD','Chad')
INSERT INTO [dbo].[Country] VALUES ('TF','French Southern Territories')
INSERT INTO [dbo].[Country] VALUES ('TG','To')
INSERT INTO [dbo].[Country] VALUES ('TH','Thailand')
INSERT INTO [dbo].[Country] VALUES ('TJ','Tajikistan')
INSERT INTO [dbo].[Country] VALUES ('TK','Tokelau')
INSERT INTO [dbo].[Country] VALUES ('TM','Turkmenistan')
INSERT INTO [dbo].[Country] VALUES ('TN','Tunisia')
INSERT INTO [dbo].[Country] VALUES ('TO','Tonga')
INSERT INTO [dbo].[Country] VALUES ('TP','East Timor')
INSERT INTO [dbo].[Country] VALUES ('TR','Turkey')
INSERT INTO [dbo].[Country] VALUES ('TT','Trinidad and Toba')
INSERT INTO [dbo].[Country] VALUES ('TV','Tuvalu')
INSERT INTO [dbo].[Country] VALUES ('TW','Taiwan')
INSERT INTO [dbo].[Country] VALUES ('TY','Mayotte')
INSERT INTO [dbo].[Country] VALUES ('TZ','Tanzania, United Republic of')
INSERT INTO [dbo].[Country] VALUES ('UA','Ukraine')
INSERT INTO [dbo].[Country] VALUES ('UG','Uganda')
INSERT INTO [dbo].[Country] VALUES ('UK','United Kingdom')
INSERT INTO [dbo].[Country] VALUES ('UM','United States minor outlying islands')
INSERT INTO [dbo].[Country] VALUES ('US','United States')
INSERT INTO [dbo].[Country] VALUES ('UY','Uruguay')
INSERT INTO [dbo].[Country] VALUES ('UZ','Uzbekistan')
INSERT INTO [dbo].[Country] VALUES ('VA','Vatican City State')
INSERT INTO [dbo].[Country] VALUES ('VC','Saint Vincent and the Grenadines')
INSERT INTO [dbo].[Country] VALUES ('VE','Venezuela')
INSERT INTO [dbo].[Country] VALUES ('VG','Virgin Islands (British)')
INSERT INTO [dbo].[Country] VALUES ('VI','Virgin Islands (U.S.)')
INSERT INTO [dbo].[Country] VALUES ('VN','Vietnam')
INSERT INTO [dbo].[Country] VALUES ('VU','Vanuatu')
INSERT INTO [dbo].[Country] VALUES ('WF','Wallis and Futuna Islands')
INSERT INTO [dbo].[Country] VALUES ('WS','Samoa')
INSERT INTO [dbo].[Country] VALUES ('XK','Kosovo')
INSERT INTO [dbo].[Country] VALUES ('YE','Yemen')
INSERT INTO [dbo].[Country] VALUES ('ZA','South Africa')
INSERT INTO [dbo].[Country] VALUES ('ZM','Zambia')
INSERT INTO [dbo].	[Country] VALUES ('ZR','Zaire')
INSERT INTO [dbo].[Country] VALUES ('ZW','Zimbabwe')
GO

CREATE SCHEMA [Identity]
GO


CREATE TABLE [Identity].[ApiResources] (
    [Id] int NOT NULL IDENTITY,
    [Enabled] bit NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [DisplayName] nvarchar(200) NULL,
    [Description] nvarchar(1000) NULL,
    [AllowedAccessTokenSigningAlgorithms] nvarchar(100) NULL,
    [ShowInDiscoveryDocument] bit NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [LastAccessed] datetime2 NULL,
    [NonEditable] bit NOT NULL,
    CONSTRAINT [PK_ApiResources] PRIMARY KEY ([Id])
);

INSERT INTO [Identity].ApiResources (Enabled, Name, DisplayName, Description, AllowedAccessTokenSigningAlgorithms, ShowInDiscoveryDocument, Created, Updated, LastAccessed, NonEditable)
VALUES (1, 'openid', 'Open ID', 'Open ID', 0, 1, GETDATE(), NULL, NULL, 0), 
(1, 'mylibrarywebsite', 'My Library Website', 'My Library Website', 0, 1, GETDATE(), NULL, NULL, 0)

DECLARE @ResourceID INT

SELECT @ResourceID = Id
FROM [Identity].ApiResources
WHERE [Name] = 'openid'


CREATE TABLE [Identity].[ApiScopes] (
    [Id] int NOT NULL IDENTITY,
    [Enabled] bit NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [DisplayName] nvarchar(200) NULL,
    [Description] nvarchar(1000) NULL,
    [Required] bit NOT NULL,
    [Emphasize] bit NOT NULL,
    [ShowInDiscoveryDocument] bit NOT NULL,
    CONSTRAINT [PK_ApiScopes] PRIMARY KEY ([Id])
);



INSERT INTO [Identity].ApiScopes (Description, DisplayName, Emphasize, Enabled, Name, Required, ShowInDiscoveryDocument)
VALUES ('Provides access to the My Library Web API', 'My Library API', 0, 1, 'mylibraryapi', 0, 1)


CREATE TABLE [Identity].[Clients] (
    [Id] int NOT NULL IDENTITY,
    [Enabled] bit NOT NULL,
    [ClientId] nvarchar(200) NOT NULL,
    [ProtocolType] nvarchar(200) NOT NULL,
    [RequireClientSecret] bit NOT NULL,
    [ClientName] nvarchar(200) NULL,
    [Description] nvarchar(1000) NULL,
    [ClientUri] nvarchar(2000) NULL,
    [LogoURI] nvarchar(2000) NULL,
    [RequireConsent] bit NOT NULL,
    [AllowRememberConsent] bit NOT NULL,
    [AlwaysIncludeUserClaimsInIdToken] bit NOT NULL,
    [RequirePkce] bit NOT NULL,
    [AllowPlainTextPkce] bit NOT NULL,
    [RequireRequestObject] bit NOT NULL,
    [AllowAccessTokensViaBrowser] bit NOT NULL,
    [FrontChannelLogoutUri] nvarchar(2000) NULL,
    [FrontChannelLogoutSessionRequired] bit NOT NULL,
    [BackChannelLogoutUri] nvarchar(2000) NULL,
    [BackChannelLogoutSessionRequired] bit NOT NULL,
    [AllowOfflineAccess] bit NOT NULL,
    [IdentityTokenLifetime] int NOT NULL,
    [AllowedIdentityTokenSigningAlgorithms] nvarchar(100) NULL,
    [AccessTokenLifetime] int NOT NULL,
    [AuthorizationCodeLifetime] int NOT NULL,
    [ConsentLifetime] int NULL,
    [AbsoluteRefreshTokenLifetime] int NOT NULL,
    [SlidingRefreshTokenLifetime] int NOT NULL,
    [RefreshTokenUsage] VARCHAR(255) NOT NULL,
    [UpdateAccessTokenClaimsOnRefresh] bit NOT NULL,
    [RefreshTokenExpiration] int NOT NULL,
    [AccessTokenType] int NOT NULL,
    [EnableLocalLogin] bit NOT NULL,
    [IncludeJwtId] bit NOT NULL,
    [AlwaysSendClientClaims] bit NOT NULL,
    [ClientClaimsPrefix] nvarchar(200) NULL,
    [PairWiseSubjectSalt] nvarchar(200) NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [LastAccessed] datetime2 NULL,
    [UserSsoLifetime] int NULL,
    [UserCodeType] nvarchar(100) NULL,
    [DeviceCodeLifetime] int NOT NULL,
    [NonEditable] bit NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);



INSERT INTO [Identity].[Clients]
           ([Enabled]
           ,[ClientId]
           ,[ProtocolType]
           ,[RequireClientSecret]
           ,[ClientName]
           ,[Description]
           ,[ClientUri]
           ,[LogoUri]
           ,[RequireConsent]
           ,[AllowRememberConsent]
           ,[AlwaysIncludeUserClaimsInIdToken]
           ,[RequirePkce]
           ,[AllowPlainTextPkce]
           ,[RequireRequestObject]
           ,[AllowAccessTokensViaBrowser]
           ,[FrontChannelLogoutUri]
           ,[FrontChannelLogoutSessionRequired]
           ,[BackChannelLogoutUri]
           ,[BackChannelLogoutSessionRequired]
           ,[AllowOfflineAccess]
           ,[IdentityTokenLifetime]
           ,[AllowedIdentityTokenSigningAlgorithms]
           ,[AccessTokenLifetime]
           ,[AuthorizationCodeLifetime]
           ,[ConsentLifetime]
           ,[AbsoluteRefreshTokenLifetime]
           ,[SlidingRefreshTokenLifetime]
           ,[RefreshTokenUsage]
           ,[UpdateAccessTokenClaimsOnRefresh]
           ,[RefreshTokenExpiration]
           ,[AccessTokenType]
           ,[EnableLocalLogin]
           ,[IncludeJwtId]
           ,[AlwaysSendClientClaims]
           ,[ClientClaimsPrefix]
           ,[PairWiseSubjectSalt]
           ,[Created]
           ,[Updated]
           ,[LastAccessed]
           ,[UserSsoLifetime]
           ,[UserCodeType]
           ,[DeviceCodeLifetime]
           ,[NonEditable])
     VALUES
           (1
           ,'mylibrarywebapp'
           ,'oidc'
           ,1
           ,'My Library Web App'
           ,'My Library Web App'
           ,'http://localhost:3000'
           ,NULL
           ,0
           ,0
           ,1
           ,1
           ,0
           ,0
           ,1
           ,'http://localhost:3000/logout'
           ,1
           ,NULL
           ,1
           ,1
           ,6000
           ,NULL
           ,6000
           ,6000
           ,6000
           ,6000
           ,6000
           ,'OneTimeOnly'
           ,1
           ,6000
           ,0
           ,1
           ,1
           ,1
           ,NULL
           ,NULL
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,6000
           ,0)

	DECLARE @ClientID INT

	SELECT @ClientID = Id
	FROM [Identity].Clients 
	WHERE ClientId = 'mylibrarywebapp'


CREATE TABLE [Identity].[IdentityResources] (
    [Id] int NOT NULL IDENTITY,
    [Enabled] bit NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [DisplayName] nvarchar(200) NULL,
    [Description] nvarchar(1000) NULL,
    [Required] bit NOT NULL,
    [Emphasize] bit NOT NULL,
    [ShowInDiscoveryDocument] bit NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [NonEditable] bit NOT NULL,
    CONSTRAINT [PK_IdentityResources] PRIMARY KEY ([Id])
); 

INSERT INTO [Identity].IdentityResources (Created, Description, DisplayName, Emphasize, Enabled, Name, NonEditable, Required, ShowInDiscoveryDocument, Updated)
VALUES (GETDATE(), 'openid', 'Open ID', 0, 1, 'openid', 0, 1, 1, NULL)
, (GETDATE(), 'User claims', 'User Claims', 0, 1, 'claims', 0, 1, 1, NULL)
, (GETDATE(), 'profile', 'User Profile', 0, 1, 'profile', 0, 1, 1, NULL)
,(GETDATE(), 'User role', 'User Role', 0, 1, 'role', 0, 1, 1, NULL)
,(GETDATE(), 'User username', 'Username', 0, 1, 'username', 0, 1, 1, NULL)
,(GETDATE(), 'User email', 'Email', 0, 1, 'email', 0, 1, 1, NULL)

CREATE TABLE [Identity].[ApiResourceClaims] (
    [Id] int NOT NULL IDENTITY,
    [Type] nvarchar(200) NOT NULL,
    [ApiResourceId] int NOT NULL,
    CONSTRAINT [PK_ApiResourceClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApiResourceClaims_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [Identity].[ApiResources] ([Id]) ON DELETE CASCADE
);

INSERT INTO [Identity].ApiResourceClaims (ApiResourceId, Type)
VALUES (@ResourceID, 'role'),
(@ResourceID, 'username')

CREATE TABLE [Identity].[ApiResourceProperties] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(250) NOT NULL,
    [Value] nvarchar(2000) NOT NULL,
    [ApiResourceId] int NOT NULL,
    CONSTRAINT [PK_ApiResourceProperties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApiResourceProperties_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [Identity].[ApiResources] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ApiResourceScopes] (
    [Id] int NOT NULL IDENTITY,
    [Scope] nvarchar(200) NOT NULL,
    [ApiResourceId] int NOT NULL,
    CONSTRAINT [PK_ApiResourceScopes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApiResourceScopes_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [Identity].[ApiResources] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Identity].[ApiResourceSecrets] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(1000) NULL,
    [Value] nvarchar(4000) NOT NULL,
    [Expiration] datetime2 NULL,
    [Type] nvarchar(250) NOT NULL,
    [Created] datetime2 NOT NULL,
    [ApiResourceId] int NOT NULL,
    CONSTRAINT [PK_ApiResourceSecrets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApiResourceSecrets_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [Identity].[ApiResources] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ApiScopeClaims] (
    [Id] int NOT NULL IDENTITY,
    [Type] nvarchar(200) NOT NULL,
    [ScopeId] int NOT NULL,
    CONSTRAINT [PK_ApiScopeClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ScopeId] FOREIGN KEY ([ScopeId]) REFERENCES [Identity].[ApiScopes] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ApiScopeProperties] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(250) NOT NULL,
    [Value] nvarchar(2000) NOT NULL,
    [ScopeId] int NOT NULL,
    CONSTRAINT [PK_ApiScopeProperties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApiScopeProperties_ApiScopes_ScopeId] FOREIGN KEY ([ScopeId]) REFERENCES [Identity].[ApiScopes] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ClientClaims] (
    [Id] int NOT NULL IDENTITY,
    [Type] nvarchar(250) NOT NULL,
    [Value] nvarchar(250) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientClaims_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ClientCorsOrigins] (
    [Id] int NOT NULL IDENTITY,
    [Origin] nvarchar(150) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientCorsOrigins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientCorsOrigins_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ClientGrantTypes] (
    [Id] int NOT NULL IDENTITY,
    [GrantType] nvarchar(250) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientGrantTypes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientGrantTypes_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



INSERT INTO [Identity].[ClientGrantTypes] (GrantType, ClientId)
VALUES ('password', @ClientID),
('client_credentials', @ClientID),
('implicit', @ClientID)

CREATE TABLE [Identity].[ClientIdPRestrictions] (
    [Id] int NOT NULL IDENTITY,
    [Provider] nvarchar(200) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientIdPRestrictions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientIdPRestrictions_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ClientPostLogoutRedirectUris] (
    [Id] int NOT NULL IDENTITY,
    [PostLogoutRedirectUri] nvarchar(2000) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientPostLoutRedirectUris] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientPostLoutRedirectUris_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[ClientProperties] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(250) NOT NULL,
    [Value] nvarchar(2000) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientProperties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientProperties_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);




CREATE TABLE [Identity].[ClientRedirectUris] (
    [Id] int NOT NULL IDENTITY,
    [RedirectUri] nvarchar(2000) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientRedirectUris] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientRedirectUris_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);

INSERT INTO [Identity].ClientRedirectUris (ClientId, RedirectUri)
VALUES (@ClientID, 'http://localhost:3000/callback')


CREATE TABLE [Identity].[ClientScopes] (
    [Id] int NOT NULL IDENTITY,
    [Scope] nvarchar(200) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientScopes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientScopes_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



INSERT INTO [Identity].ClientScopes (ClientId, Scope)
VALUES (@ClientID, 'openid'),
(@ClientID, 'profile'),
(@ClientID, 'mylibraryapi'),
(@ClientID, 'role'),
(@ClientID, 'username'),
(@ClientID, 'email')


CREATE TABLE [Identity].[ClientSecrets] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(2000) NULL,
    [Value] nvarchar(4000) NOT NULL,
    [Expiration] datetime2 NULL,
    [Type] nvarchar(250) NOT NULL,
    [Created] datetime2 NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_ClientSecrets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientSecrets_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Identity].[Clients] ([Id]) ON DELETE CASCADE
);



INSERT INTO [Identity].ClientSecrets (ClientId, Created, Description, Expiration, Type, Value)
VALUES (@ClientID, GETDATE(), 'mylibrarywebsite', NULL, 'SharedSecret', '979eb386dc9a387d614b72902e44f5cb295636d71f829d2afccff401eb794bd6')


CREATE TABLE [Identity].[IdentityResourceClaims] (
    [Id] int NOT NULL IDENTITY,
    [Type] nvarchar(200) NOT NULL,
    [IdentityResourceId] int NOT NULL,
    CONSTRAINT [PK_IdentityResourceClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IdentityResourceClaims_IdentityResources_IdentityResourceId] FOREIGN KEY ([IdentityResourceId]) REFERENCES [Identity].[IdentityResources] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [Identity].[IdentityResourceProperties] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(250) NOT NULL,
    [Value] nvarchar(2000) NOT NULL,
    [IdentityResourceId] int NOT NULL,
    CONSTRAINT [PK_IdentityResourceProperties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IdentityResourceProperties_IdentityResources_IdentityResourceId] FOREIGN KEY ([IdentityResourceId]) REFERENCES [Identity].[IdentityResources] ([Id]) ON DELETE CASCADE
);



CREATE INDEX [IX_ApiResourceClaims_ApiResourceId] ON [Identity].[ApiResourceClaims] ([ApiResourceId]);



CREATE INDEX [IX_ApiResourceProperties_ApiResourceId] ON [Identity].[ApiResourceProperties] ([ApiResourceId]);



CREATE UNIQUE INDEX [IX_ApiResources_Name] ON [Identity].[ApiResources] ([Name]);



CREATE INDEX [IX_ApiResourceScopes_ApiResourceId] ON [Identity].[ApiResourceScopes] ([ApiResourceId]);



CREATE INDEX [IX_ApiResourceSecrets_ApiResourceId] ON [Identity].[ApiResourceSecrets] ([ApiResourceId]);



CREATE INDEX [IX_ApiScopeClaims_ScopeId] ON [Identity].[ApiScopeClaims] ([ScopeId]);



CREATE INDEX [IX_ApiScopeProperties_ScopeId] ON [Identity].[ApiScopeProperties] ([ScopeId]);



CREATE UNIQUE INDEX [IX_ApiScopes_Name] ON [Identity].[ApiScopes] ([Name]);



CREATE INDEX [IX_ClientClaims_ClientId] ON [Identity].[ClientClaims] ([ClientId]);



CREATE INDEX [IX_ClientCorsOrigins_ClientId] ON [Identity].[ClientCorsOrigins] ([ClientId]);



CREATE INDEX [IX_ClientGrantTypes_ClientId] ON [Identity].[ClientGrantTypes] ([ClientId]);



CREATE INDEX [IX_ClientIdPRestrictions_ClientId] ON [Identity].[ClientIdPRestrictions] ([ClientId]);



CREATE INDEX [IX_ClientPostLogoutRedirectUris_ClientId] ON [Identity].[ClientPostLogoutRedirectUris] ([ClientId]);



CREATE INDEX [IX_ClientProperties_ClientId] ON [Identity].[ClientProperties] ([ClientId]);



CREATE INDEX [IX_ClientRedirectUris_ClientId] ON [Identity].[ClientRedirectUris] ([ClientId]);



CREATE UNIQUE INDEX [IX_Clients_ClientId] ON [Identity].[Clients] ([ClientId]);



CREATE INDEX [IX_ClientScopes_ClientId] ON [Identity].[ClientScopes] ([ClientId]);



CREATE INDEX [IX_ClientSecrets_ClientId] ON [Identity].[ClientSecrets] ([ClientId]);



CREATE INDEX [IX_IdentityResourceClaims_IdentityResourceId] ON [Identity].[IdentityResourceClaims] ([IdentityResourceId]);



CREATE INDEX [IX_IdentityResourceProperties_IdentityResourceId] ON [Identity].[IdentityResourceProperties] ([IdentityResourceId]);



CREATE UNIQUE INDEX [IX_IdentityResources_Name] ON [Identity].[IdentityResources] ([Name]);



CREATE TABLE [Identity].[DeviceCodes] (
    [UserCode] nvarchar(200) NOT NULL,
    [DeviceCode] nvarchar(200) NOT NULL,
    [SubjectId] nvarchar(200) NULL,
    [SessionId] nvarchar(100) NULL,
    [ClientId] nvarchar(200) NOT NULL,
    [Description] nvarchar(200) NULL,
    [CreationTime] datetime2 NOT NULL,
    [Expiration] datetime2 NOT NULL,
    [Data] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_DeviceCodes] PRIMARY KEY ([UserCode])
);



CREATE TABLE [Identity].[PersistedGrants] (
    [Key] nvarchar(200) NOT NULL,
    [Type] nvarchar(50) NOT NULL,
    [SubjectId] nvarchar(200) NULL,
    [SessionId] nvarchar(100) NULL,
    [ClientId] nvarchar(200) NOT NULL,
    [Description] nvarchar(200) NULL,
    [CreationTime] datetime2 NOT NULL,
    [Expiration] datetime2 NULL,
    [ConsumedTime] datetime2 NULL,
    [Data] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PersistedGrants] PRIMARY KEY ([Key])
);



CREATE UNIQUE INDEX [IX_DeviceCodes_DeviceCode] ON [Identity].[DeviceCodes] ([DeviceCode]);



CREATE INDEX [IX_DeviceCodes_Expiration] ON [Identity].[DeviceCodes] ([Expiration]);



CREATE INDEX [IX_PersistedGrants_Expiration] ON [Identity].[PersistedGrants] ([Expiration]);



CREATE INDEX [IX_PersistedGrants_SubjectId_ClientId_Type] ON [Identity].[PersistedGrants] ([SubjectId], [ClientId], [Type]);



CREATE INDEX [IX_PersistedGrants_SubjectId_SessionId_Type] ON [Identity].[PersistedGrants] ([SubjectId], [SessionId], [Type]);


