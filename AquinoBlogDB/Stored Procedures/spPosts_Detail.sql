CREATE PROCEDURE [dbo].[spPosts_Detail]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [p].[Id], [p].[Title], [p].[Body], [p].[DateCreated], 
	[u].[UserName], [u].[FirstName], [u].[LastName]
	FROM dbo.Posts p
	INNER JOIN dbo.Users u
	ON p.UserId = u.id
	WHERE p.Id = @id;

END