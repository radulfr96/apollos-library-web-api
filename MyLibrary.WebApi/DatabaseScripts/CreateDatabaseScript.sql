USE [master]

DROP DATABASE MyLibrary

CREATE DATABASE MyLibrary
GO

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

CREATE TABLE [Users].[User] 
(
	[UserID] INT IDENTITY NOT NULL,
	[Username] VARCHAR(20) NOT NULL,
	[Password] TEXT NOT NULL,
	[Salter] TEXT NOT NULL,
	[IsActive] BIT NOT NULL,
	[IsDeleted] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] VARCHAR(20) NOT NULL,
	[ModifiedDate] DATETIME,
	[ModifiedBy] INT,
	CONSTRAINT PK_User PRIMARY KEY (UserID)
)

CREATE TABLE [Users].[Role]
(
	[RoleID] INT IDENTITY NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	CONSTRAINT PK_Role PRIMARY KEY (RoleID)
)

CREATE TABLE [Users].[UserRole]
(
	[UserRoleID] INT IDENTITY NOT NULL,
	[UserID] INT NOT NULL,
	[RoleID] INT NOT NULL,
	CONSTRAINT PK_UserRole PRIMARY KEY (UserRoleID),
	CONSTRAINT FK_UserRoleUser FOREIGN KEY (UserID)
	REFERENCES [Users].[User] (UserID),
	CONSTRAINT FK_UserRoleRole FOREIGN KEY (RoleID)
	REFERENCES [Users].[Role] (RoleID)
)

CREATE TABLE [Genre].[Genre]
(
	[GenreID] INT IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] INT,
	[ModifiedDate] DATETIME,
	[ModifiedBy] INT,
	CONSTRAINT PK_Genre PRIMARY KEY (GenreID),
	CONSTRAINT FK_GenreUser
	FOREIGN KEY (CreatedBy) REFERENCES [Users].[User] (UserID),
	CONSTRAINT FK_GenreUserModified
	FOREIGN KEY (ModifiedBy) REFERENCES [Users].[User] (UserID)
)
GO

CREATE TABLE [Book].[Series]
(
	[SeriesID] INT IDENTITY NOT NULL,
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
	[Postcode] VARCHAR(5) NOT NULL,
	[State] VARCHAR(50) NOT NULL,
	[CountryID] CHAR(2) NOT NULL,
	[IsDeleted] BIT,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] INT,
	[ModifiedDate] DATETIME,
	[ModifiedBy] INT,
	CONSTRAINT FK_PublisherCountry
	FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] (CountryID),
	CONSTRAINT FK_PublisherUser
	FOREIGN KEY (CreatedBy) REFERENCES [Users].[User] (UserID),
	CONSTRAINT FK_PublisherUserModified
	FOREIGN KEY (ModifiedBy) REFERENCES [Users].[User] (UserID)
)
GO

CREATE TABLE [Author].[Author]
(
	[AuthorID] INT PRIMARY KEY IDENTITY NOT NULL,
	[FirstName] VARCHAR(50) NOT NULL,
	[MiddleName] VARCHAR(50),
	[LastName] VARCHAR(50),
	[Description] VARCHAR(2000),
	[CountryID] CHAR(2) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] INT NOT NULL,
	[ModifiedDate] DATETIME,
	[ModifiedBy] INT,
	CONSTRAINT FK_AuthorCountry
	FOREIGN KEY (CountryID) REFERENCES [dbo].[Country] (CountryID) 
)

CREATE TABLE [Book].[Book]
(
	[BookID] INT IDENTITY NOT NULL,
	[ISBN] VARCHAR(12),
	[eISBN] VARCHAR(12),
	[Title] VARCHAR(200) NOT NULL,
	[Subtitle] VARCHAR(200),
	[SeriesID] INT,
	[NumberInSeries] INT,
	[Edition] INT,
	[PublicationFormatID] INT NOT NULL,
	[FictionTypeID] INT NOT NULL,
	[FormTypeID] INT NOT NULL,
	[PublisherID] INT NOT NULL,
	[CoverImage] TEXT,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] INT NOT NULL,
	[ModifiedDate] DATETIME,
	[ModifiedBy] INT,
	CONSTRAINT PK_Book PRIMARY KEY (BookID),
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
	FOREIGN KEY (CreatedBy)
	REFERENCES [Users].[User] (UserID),
	CONSTRAINT FK_PublisherUserModified
	FOREIGN KEY (ModifiedBy) 
	REFERENCES [Users].[User] (UserID)
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

DECLARE @UserID INT
, @AdminRoleID INT
, @StandardRoleID INT


INSERT INTO [Users].[User] ([Username], [Password], [Salter], [CreatedDate], [CreatedBy], [IsActive], [IsDeleted])
VALUES ('Radulfr', 'BvsurrjL2gS75K9KhRSbJneH3//7qCQRlmpTZF7JGs4=', 'Q3uQu0Nybf8Jpb6suzJPsQ==', GETDATE(), 'Radulfr', 1, 0)

SELECT @UserID = U.UserID
FROM Users.[User] U
WHERE U.Username = 'Radulfr'

INSERT INTO [Users].[Role] ([Name])
VALUES ('Admin'), ('Standard User')

SELECT @AdminRoleID = R.RoleID
FROM Users.[Role] R
WHERE R.[Name] = 'Admin'

SELECT @StandardRoleID = R.RoleID
FROM Users.[Role] R
WHERE R.[Name] = 'Standard User'

INSERT INTO [Users].[UserRole] ([UserID], [RoleID])
VALUES (@UserID, @StandardRoleID)

INSERT INTO [Users].[UserRole] ([UserID], [RoleID])
VALUES (@UserID, @AdminRoleID)
GO

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
INSERT INTO [dbo].[Country] VALUES ('AO','Angola')
INSERT INTO [dbo].[Country] VALUES ('AQ','Antarctica')
INSERT INTO [dbo].[Country] VALUES ('AR','Argentina')
INSERT INTO [dbo].[Country] VALUES ('AT','Austria')
INSERT INTO [dbo].[Country] VALUES ('AU','Australia')
INSERT INTO [dbo].[Country] VALUES ('AW','Aruba')
INSERT INTO [dbo].[Country] VALUES ('AZ','Azerbaijan')
INSERT INTO [dbo].[Country] VALUES ('BA','Bosnia and Herzegovina')
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
INSERT INTO [dbo].[Country] VALUES ('CG','Congo')
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
INSERT INTO [dbo].[Country] VALUES ('MN','Mongolia')
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
INSERT INTO [dbo].[Country] VALUES ('TG','Togo')
INSERT INTO [dbo].[Country] VALUES ('TH','Thailand')
INSERT INTO [dbo].[Country] VALUES ('TJ','Tajikistan')
INSERT INTO [dbo].[Country] VALUES ('TK','Tokelau')
INSERT INTO [dbo].[Country] VALUES ('TM','Turkmenistan')
INSERT INTO [dbo].[Country] VALUES ('TN','Tunisia')
INSERT INTO [dbo].[Country] VALUES ('TO','Tonga')
INSERT INTO [dbo].[Country] VALUES ('TP','East Timor')
INSERT INTO [dbo].[Country] VALUES ('TR','Turkey')
INSERT INTO [dbo].[Country] VALUES ('TT','Trinidad and Tobago')
INSERT INTO [dbo].[Country] VALUES ('TV','Tuvalu')
INSERT INTO [dbo].[Country] VALUES ('TW','Taiwan')
INSERT INTO [dbo].[Country] VALUES ('TY','Mayotte')
INSERT INTO [dbo].[Country] VALUES ('TZ','Tanzania, United Republic of')
INSERT INTO [dbo].[Country] VALUES ('UA','Ukraine')
INSERT INTO [dbo].[Country] VALUES ('UG','Uganda')
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