CREATE PROCEDURE [dbo].[SwitchRoleUser]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	IF((SELECT IsAdmin FROM [dbo].[UserApp] WHERE Id = @Id) = 1)
	BEGIN
		UPDATE [dbo].[UserApp] SET IsAdmin = 0 WHERE Id = @Id
	END
	ELSE
	BEGIN
		UPDATE [dbo].[UserApp] SET IsAdmin = 1 WHERE Id = @Id
	END
END
