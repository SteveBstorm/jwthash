CREATE PROCEDURE [dbo].[LoginUser]
	@Email NVARCHAR(250),
	@Password NVARCHAR(50)
AS
BEGIN
	-- Recuperation du salt lier a l'email
	DECLARE @Salt NVARCHAR(100);
	SELECT @Salt = Salt FROM [dbo].[UserApp] WHERE [Email] LIKE @Email;

	-- On continue sur on a trouvé un salt
	IF @Salt IS NOT NULL
	BEGIN
		-- Recuperation de la valeur secrete
		DECLARE @Secret NVARCHAR(50);
		SET @Secret = [dbo].[GetSecretKey]();

		-- On hash le mot de passe recu pour pouvoir réaliser la comparaison
		DECLARE @Password_Hash VARBINARY(64);
		SET @Password_Hash = HASHBYTES('SHA2_512', CONCAT(@Salt, @Password, @Secret, @Salt));

		-- On souhaite obtenir l'id de l'utilisateur sur base de l'email et du password hashé
		DECLARE @UserId UNIQUEIDENTIFIER;
		SELECT @UserId = [Id] FROM [UserApp] WHERE [Email] LIKE @Email AND Password = @Password_Hash;

		-- Renvoie de l'utilisateur trouvé via la vue "V_UserApp"
		SELECT * FROM [V_UserApp] WHERE [Id] = @UserId;
	END
END