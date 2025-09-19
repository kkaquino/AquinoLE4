CREATE PROCEDURE [dbo].[spPosts_Insert]
    @userId int,
    @title nvarchar(50),
    @body text,
    @dateCreated datetime2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Posts
        (UserId, Title, Body, DateCreated)
    VALUES
        (@userId, @title, @body, @dateCreated);
END
