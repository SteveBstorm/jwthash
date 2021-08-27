/*
Script de déploiement pour DemoToken

Ce code a été généré par un outil.
La modification de ce fichier peut provoquer un comportement incorrect et sera perdue si
le code est régénéré.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DemoToken"
:setvar DefaultFilePrefix "DemoToken"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL12.TFTIC2014\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL12.TFTIC2014\MSSQL\DATA\"

GO
:on error exit
GO
/*
Détectez le mode SQLCMD et désactivez l'exécution du script si le mode SQLCMD n'est pas pris en charge.
Pour réactiver le script une fois le mode SQLCMD activé, exécutez ce qui suit :
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Le mode SQLCMD doit être activé de manière à pouvoir exécuter ce script.';
        SET NOEXEC ON;
    END


GO
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Création de $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [$(DatabaseName)], FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix)_Primary.mdf')
    LOG ON (NAME = [$(DatabaseName)_log], FILENAME = N'$(DefaultLogPath)$(DefaultFilePrefix)_Primary.ldf') COLLATE SQL_Latin1_General_CP1_CI_AS
GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'Impossible de modifier les paramètres de base de données. Vous devez être administrateur système pour appliquer ces paramètres.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'Impossible de modifier les paramètres de base de données. Vous devez être administrateur système pour appliquer ces paramètres.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF),
                MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = OFF,
                DELAYED_DURABILITY = DISABLED 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
PRINT N'Création de [dbo].[Message]...';


GO
CREATE TABLE [dbo].[Message] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Title]       NVARCHAR (50)    NOT NULL,
    [Content]     NVARCHAR (1000)  NOT NULL,
    [DateMessage] DATETIME2 (7)    NOT NULL,
    [UserAppId]   UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Création de [dbo].[UserApp]...';


GO
CREATE TABLE [dbo].[UserApp] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Email]    NVARCHAR (250)   NOT NULL,
    [Username] NVARCHAR (50)    NOT NULL,
    [Password] VARBINARY (64)   NOT NULL,
    [Salt]     NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_UserApp] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_UserApp_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [UK_UserApp_Username] UNIQUE NONCLUSTERED ([Username] ASC)
);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[Message]...';


GO
ALTER TABLE [dbo].[Message]
    ADD DEFAULT NEWSEQUENTIALID() FOR [Id];


GO
PRINT N'Création de contrainte sans nom sur [dbo].[Message]...';


GO
ALTER TABLE [dbo].[Message]
    ADD DEFAULT GETDATE() FOR [DateMessage];


GO
PRINT N'Création de contrainte sans nom sur [dbo].[UserApp]...';


GO
ALTER TABLE [dbo].[UserApp]
    ADD DEFAULT NEWSEQUENTIALID() FOR [Id];


GO
PRINT N'Création de [dbo].[FK_Message_User]...';


GO
ALTER TABLE [dbo].[Message]
    ADD CONSTRAINT [FK_Message_User] FOREIGN KEY ([UserAppId]) REFERENCES [dbo].[UserApp] ([Id]) ON DELETE SET NULL;


GO
PRINT N'Création de [dbo].[V_UserApp]...';


GO
CREATE VIEW [dbo].[V_UserApp]
AS
SELECT [Id], [Email], [Username]
FROM [dbo].[UserApp]
GO
PRINT N'Création de [dbo].[GetSecretKey]...';


GO
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
GO
PRINT N'Création de [dbo].[AddMessage]...';


GO
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
GO
PRINT N'Création de [dbo].[AddUser]...';


GO
CREATE PROCEDURE [dbo].[AddUser]
	@Email NVARCHAR(250),
	@Username NVARCHAR(50),
	@Password NVARCHAR(50)
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
	INSERT INTO [dbo].[UserApp] ([Email], [Username], [Password], [Salt])
	 OUTPUT [inserted].[Id]
	 VALUES (@Email, @Username, @Password_hash, @Salt);
END
GO
PRINT N'Création de [dbo].[LoginUser]...';


GO
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
GO

EXEC [dbo].[AddUser] 'b.picsou@outlook.com', 'Balthazar', 'Test1234=';
EXEC [dbo].[AddUser] 'z.vanderquack@outlook.com', 'Zaza', 'Test1234=';
GO

GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
PRINT N'Mise à jour terminée.';


GO
