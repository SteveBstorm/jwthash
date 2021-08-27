CREATE FUNCTION [dbo].[GetSecretKey]()
RETURNS NVARCHAR(50)
AS
BEGIN
	-- Declaration 
	DECLARE @key VARCHAR(50);
	
	-- Generation de la valeur
	SET @key = '&N*%zh/2ZM?!3n#J.wWH9%3UpDvDq%v$R!;84ew+q+%vr)FPbH';

	-- Renvoie du Salt
	RETURN @key;
END
