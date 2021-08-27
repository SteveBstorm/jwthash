CREATE PROCEDURE [dbo].[UpdateMessage]
	@Id UNIQUEIDENTIFIER,
	@Title NVARCHAR(50),
	@Content NVARCHAR(1000)
AS
BEGIN
	UPDATE [dbo].[Message] SET Title = @Title, Content = @Content WHERE Id = @Id
END