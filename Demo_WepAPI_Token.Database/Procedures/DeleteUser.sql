﻿CREATE PROCEDURE [dbo].[DeleteUser]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM [dbo].[UserApp] WHERE Id = @Id
END
