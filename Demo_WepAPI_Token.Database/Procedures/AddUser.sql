CREATE PROCEDURE [dbo].[AddUser]
	@Email NVARCHAR(250),
	@Username NVARCHAR(50),
	@Password NVARCHAR(50),
	@IsAdmin BIT
AS
BEGIN
	SET NOCOUNT ON;

	-- Generation du Salt
	DECLARE @Salt NVARCHAR(100);
	SET @Salt = CONCAT(NEWID(),NEWID(),NEWID());

	-- Recuperation de la valeur secrete
	DECLARE @Secret NVARCHAR(50);
	SET @Secret = [dbo].[GetSecretKey]();

	-- Hashage du mot de passe avec le salt
	DECLARE @Password_hash VARBINARY(64);
	SET @Password_hash = HASHBYTES('SHA2_512', CONCAT(@Salt, @Password, @Secret, @Salt));

	-- Ajout de l'utilisateur dans la DB avec le mot de passe hashé
	INSERT INTO [dbo].[UserApp] ([Email], [Username], [Password], [Salt], [IsAdmin])
	 OUTPUT [inserted].[Id]
	 VALUES (@Email, @Username, @Password_hash, @Salt, @IsAdmin);
END