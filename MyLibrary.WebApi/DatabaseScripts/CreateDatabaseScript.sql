CREATE DATABASE MyLibrary
GO

USE MyLibrary
GO

CREATE SCHEMA [Users]
GO

DECLARE @UserID INT
, @RoleID INT

CREATE TABLE [Users].[User] 
(
	[UserID] INT IDENTITY,
	[Username] VARCHAR(20) UNIQUE,
	[Password] TEXT,
	[Salter] TEXT,
	[SetPassword] BIT,
	CONSTRAINT PK_User PRIMARY KEY (UserID)
)

INSERT INTO [Users].[User] ([Username], [Password], [SetPassword], [Salter])
VALUES ('Radulfr', 'BvsurrjL2gS75K9KhRSbJneH3//7qCQRlmpTZF7JGs4=', 0, 'Q3uQu0Nybf8Jpb6suzJPsQ==')

SELECT @UserID = U.UserID
FROM Users.[User] U
WHERE U.Username = 'Radulfr'

CREATE TABLE [Users].[Role] 
(
	[RoleID] INT IDENTITY,
	[Name] VARCHAR(20),
	CONSTRAINT PK_Role PRIMARY KEY (RoleID)
)

INSERT INTO [Users].[Role] ([Name])
VALUES ('Admin'), ('Starndar User')

SELECT @RoleID = R.RoleID
FROM Users.[Role] R
WHERE R.[Name] = 'Admin'

CREATE TABLE [Users].[UserRole]
(
	[UserRoleID] INT IDENTITY,
	[UserID] INT,
	[RoleID] INT,
	CONSTRAINT PK_UserRole PRIMARY KEY (UserRoleID),
	CONSTRAINT FK_UserRoleUser FOREIGN KEY (UserRoleID)
	REFERENCES [Users].[User] (UserID),
	CONSTRAINT FK_UserRoleRole FOREIGN KEY (RoleID)
	REFERENCES [Users].[Role] (RoleID)
)
INSERT INTO [Users].[UserRole] ([UserID], [RoleID])
VALUES (@UserID, @RoleID)

USE MASTER
DROP DATABASE MyLibrary