USE [master]

DROP DATABASE MyLibrary

CREATE DATABASE MyLibrary
GO

USE MyLibrary
GO

CREATE SCHEMA [Users]
GO

DECLARE @UserID INT
, @AdminRoleID INT
, @StandardRoleID INT

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
	[ModifiedBy] VARCHAR(20),
	CONSTRAINT PK_User PRIMARY KEY (UserID)
)

INSERT INTO [Users].[User] ([Username], [Password], [Salter], [CreatedDate], [CreatedBy], [IsActive], [IsDeleted])
VALUES ('Radulfr', 'BvsurrjL2gS75K9KhRSbJneH3//7qCQRlmpTZF7JGs4=', 'Q3uQu0Nybf8Jpb6suzJPsQ==', GETDATE(), 'Radulfr', 1, 1)

SELECT @UserID = U.UserID
FROM Users.[User] U
WHERE U.Username = 'Radulfr'

CREATE TABLE [Users].[Role]
(
	[RoleID] INT IDENTITY NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	CONSTRAINT PK_Role PRIMARY KEY (RoleID)
)

INSERT INTO [Users].[Role] ([Name])
VALUES ('Admin'), ('Standard User')

SELECT @AdminRoleID = R.RoleID
FROM Users.[Role] R
WHERE R.[Name] = 'Admin'

SELECT @StandardRoleID = R.RoleID
FROM Users.[Role] R
WHERE R.[Name] = 'Standard User'

CREATE TABLE [Users].[UserRole]
(
	[UserRoleID] INT IDENTITY,
	[UserID] INT NOT NULL,
	[RoleID] INT NOT NULL,
	CONSTRAINT PK_UserRole PRIMARY KEY (UserRoleID),
	CONSTRAINT FK_UserRoleUser FOREIGN KEY (UserID)
	REFERENCES [Users].[User] (UserID),
	CONSTRAINT FK_UserRoleRole FOREIGN KEY (RoleID)
	REFERENCES [Users].[Role] (RoleID)
)

PRINT ('UserID - ' + CONVERT(VARCHAR(5), @UserID))

PRINT ('RoleID - ' + + CONVERT(VARCHAR(5), @StandardRoleID))

INSERT INTO [Users].[UserRole] ([UserID], [RoleID])
VALUES (@UserID, @StandardRoleID)

PRINT ('UserID - ' + + CONVERT(VARCHAR(5), @UserID))

PRINT ('RoleID - ' + + CONVERT(VARCHAR(5), @AdminRoleID))

INSERT INTO [Users].[UserRole] ([UserID], [RoleID])
VALUES (@UserID, @AdminRoleID)
GO

CREATE SCHEMA [Genre]
GO

CREATE TABLE [Genre].[Genre]
(
	[GenreID] INT IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	[CreateDate] DATETIME NOT NULL,
	[CreatedBy] VARCHAR(500),
	[ModifiedDate] DATETIME,
	[ModifiedBy] VARCHAR(500),
	CONSTRAINT PK_Genre PRIMARY KEY (GenreID),
)
GO

CREATE SCHEMA [Book]
GO

CREATE TABLE [Book].[Book]
(
	[ISBN] VARCHAR(12) PRIMARY KEY,
	[Name] VARCHAR(200) NOT NULL,
)

CREATE TABLE [Book].[BookGenre]
(
	[GenreID] INT,
	ISBN VARCHAR(12),
	CONSTRAINT PK_BookGenre PRIMARY KEY (GenreID, ISBN),
	CONSTRAINT FK_BookGenreGenre FOREIGN KEY (GenreID)
	REFERENCES [Genre].[Genre] (GenreID),
	CONSTRAINT FK_BookGenreBook FOREIGN KEY (ISBN)
	REFERENCES [Book].[Book] (ISBN)
)