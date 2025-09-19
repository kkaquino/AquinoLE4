CREATE TABLE [dbo].[Users]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [UserName] NVARCHAR(16) NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(16) NOT NULL
)
