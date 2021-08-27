CREATE PROCEDURE [dbo].[AddMessage]
	@UserId UNIQUEIDENTIFIER,
	@Title NVARCHAR(50),
	@Content NVARCHAR(1000)
AS
BEGIN
	INSERT INTO [dbo].[Message] ([Title], [Content], [UserAppId])
	 OUTPUT [inserted].[Id]
	 VALUES (@Title, @Content, @UserId);
END