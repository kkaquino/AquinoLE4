CREATE TABLE [dbo].[Posts]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [Title] NVARCHAR(150) NOT NULL, 
    [Body] TEXT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Posts_Users] FOREIGN KEY (UserId) REFERENCES Users([Id]) 
)
