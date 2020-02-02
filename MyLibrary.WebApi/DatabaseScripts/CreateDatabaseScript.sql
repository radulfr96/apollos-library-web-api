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
	[Username] VARCHAR(20) UNIQUE NOT NULL,
	[Password] TEXT NOT NULL,
	[Salter] TEXT NOT NULL,
	[SetPassword] BIT NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[CreatedBy] VARCHAR(20) NOT NULL,
	[ModifiedDate] DATETIME,
	[ModifiedBy] VARCHAR(20),
	CONSTRAINT PK_User PRIMARY KEY (UserID)
)

INSERT INTO [Users].[User] ([Username], [Password], [SetPassword], [Salter], [CreatedDate], [CreatedBy], [IsActive])
VALUES ('Radulfr', 'BvsurrjL2gS75K9KhRSbJneH3//7qCQRlmpTZF7JGs4=', 0, 'Q3uQu0Nybf8Jpb6suzJPsQ==', GETDATE(), 'Radulfr', 1)

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


USE MASTER
DROP DATABASE MyLibrary