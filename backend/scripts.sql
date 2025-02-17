USE [master]
GO
/****** Object:  Database [prueba_viamatica]    Script Date: 16/2/2025 03:25:14 ******/
CREATE DATABASE [prueba_viamatica]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'prueba_viamatica', FILENAME = N'/var/opt/mssql/data/prueba_viamatica.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'prueba_viamatica_log', FILENAME = N'/var/opt/mssql/data/prueba_viamatica_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [prueba_viamatica] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [prueba_viamatica].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [prueba_viamatica] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [prueba_viamatica] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [prueba_viamatica] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [prueba_viamatica] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [prueba_viamatica] SET ARITHABORT OFF 
GO
ALTER DATABASE [prueba_viamatica] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [prueba_viamatica] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [prueba_viamatica] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [prueba_viamatica] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [prueba_viamatica] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [prueba_viamatica] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [prueba_viamatica] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [prueba_viamatica] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [prueba_viamatica] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [prueba_viamatica] SET  DISABLE_BROKER 
GO
ALTER DATABASE [prueba_viamatica] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [prueba_viamatica] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [prueba_viamatica] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [prueba_viamatica] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [prueba_viamatica] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [prueba_viamatica] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [prueba_viamatica] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [prueba_viamatica] SET RECOVERY FULL 
GO
ALTER DATABASE [prueba_viamatica] SET  MULTI_USER 
GO
ALTER DATABASE [prueba_viamatica] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [prueba_viamatica] SET DB_CHAINING OFF 
GO
ALTER DATABASE [prueba_viamatica] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [prueba_viamatica] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [prueba_viamatica] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [prueba_viamatica] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'prueba_viamatica', N'ON'
GO
ALTER DATABASE [prueba_viamatica] SET QUERY_STORE = ON
GO
ALTER DATABASE [prueba_viamatica] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [prueba_viamatica]
GO
/****** Object:  Table [dbo].[PERSONS]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERSONS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[names] [nvarchar](60) NOT NULL,
	[surnames] [nvarchar](60) NOT NULL,
	[identification] [nvarchar](10) NOT NULL,
	[birth_date] [datetime] NOT NULL,
 CONSTRAINT [PK__Persons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLE_OPTIONS]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLE_OPTIONS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[status] [nvarchar](20) NOT NULL,
	[link] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK__Rol_Options] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLES]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[status] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK__Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLES_ROLE_OPTIONS]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES_ROLE_OPTIONS](
	[id_role] [int] NOT NULL,
	[id_role_option] [int] NOT NULL,
 CONSTRAINT [PK__Roles__Rol_Options] PRIMARY KEY CLUSTERED 
(
	[id_role] ASC,
	[id_role_option] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLES_USERS]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES_USERS](
	[id_role] [int] NOT NULL,
	[id_user] [int] NOT NULL,
 CONSTRAINT [PK__Roles__Users] PRIMARY KEY CLUSTERED 
(
	[id_role] ASC,
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SESSIONS]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SESSIONS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[entry_date] [datetime] NULL,
	[close_date] [datetime] NULL,
	[id_user] [int] NOT NULL,
	[status] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK__SESSIONS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](20) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[mail] [nvarchar](120) NOT NULL,
	[session_active] [bit] NOT NULL,
	[status] [nvarchar](20) NOT NULL,
	[id_person] [int] NOT NULL,
 CONSTRAINT [PK__Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX__Persons__Identification]    Script Date: 16/2/2025 03:25:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX__Persons__Identification] ON [dbo].[PERSONS]
(
	[identification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX__Users__Mail]    Script Date: 16/2/2025 03:25:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX__Users__Mail] ON [dbo].[USERS]
(
	[mail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX__Users__Username]    Script Date: 16/2/2025 03:25:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX__Users__Username] ON [dbo].[USERS]
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ROLES_ROLE_OPTIONS]  WITH CHECK ADD  CONSTRAINT [FK__Roles__Role_Options__Role_Options] FOREIGN KEY([id_role_option])
REFERENCES [dbo].[ROLE_OPTIONS] ([id])
GO
ALTER TABLE [dbo].[ROLES_ROLE_OPTIONS] CHECK CONSTRAINT [FK__Roles__Role_Options__Role_Options]
GO
ALTER TABLE [dbo].[ROLES_ROLE_OPTIONS]  WITH CHECK ADD  CONSTRAINT [FK__Roles__Role_Options__Roles] FOREIGN KEY([id_role])
REFERENCES [dbo].[ROLES] ([id])
GO
ALTER TABLE [dbo].[ROLES_ROLE_OPTIONS] CHECK CONSTRAINT [FK__Roles__Role_Options__Roles]
GO
ALTER TABLE [dbo].[ROLES_USERS]  WITH CHECK ADD  CONSTRAINT [FK__Roles_Users__Roles] FOREIGN KEY([id_role])
REFERENCES [dbo].[ROLES] ([id])
GO
ALTER TABLE [dbo].[ROLES_USERS] CHECK CONSTRAINT [FK__Roles_Users__Roles]
GO
ALTER TABLE [dbo].[ROLES_USERS]  WITH CHECK ADD  CONSTRAINT [FK__Roles_Users__Users] FOREIGN KEY([id_user])
REFERENCES [dbo].[USERS] ([id])
GO
ALTER TABLE [dbo].[ROLES_USERS] CHECK CONSTRAINT [FK__Roles_Users__Users]
GO
ALTER TABLE [dbo].[SESSIONS]  WITH CHECK ADD  CONSTRAINT [FK__Sessions__Users] FOREIGN KEY([id_user])
REFERENCES [dbo].[USERS] ([id])
GO
ALTER TABLE [dbo].[SESSIONS] CHECK CONSTRAINT [FK__Sessions__Users]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK__Users__Persons] FOREIGN KEY([id_person])
REFERENCES [dbo].[PERSONS] ([id])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK__Users__Persons]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_PERSON_USER]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[INSERT_PERSON_USER]
    -- Person Params
	@Names NVARCHAR(60),
    @surnames NVARCHAR(60),
	@Identification NVARCHAR(10),
	@BirthDate DATETIME,

	-- User Params
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
	@Mail NVARCHAR(120),
	@SessionActive BIT,
	@Status NVARCHAR(20),

	-- Role Params
	@IdRoles NVARCHAR(200),

	@IdPerson INT OUTPUT,
	@IdUser INT OUTPUT
AS
BEGIN
	DECLARE @MailCount INT;
	DECLARE @FirstMailPart NVARCHAR(50);
	DECLARE @SecondMailPart NVARCHAR(50);

    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if identification exists on PERSONS
        IF EXISTS (SELECT 1 FROM PERSONS WHERE identification = @Identification)
        BEGIN
            THROW 50001, 'The identification number already exists in the system.', 1;
        END

        -- Check if username exists on USERS
        IF EXISTS (SELECT 1 FROM USERS WHERE username = @Username)
        BEGIN
            THROW 50002, 'The username already exists in the system.', 1;
        END

        -- Check if mail exists on USERS
		SET @FirstMailPart = LEFT(@Mail, CHARINDEX('@', @Mail) - 1);
		SET @SecondMailPart = RIGHT(@Mail, LEN(@Mail) - CHARINDEX('@', @Mail));

		SELECT @MailCount = COUNT(*) FROM USERS WHERE mail LIKE @FirstMailPart + '%';

        IF @MailCount >= 1
        BEGIN
            SET @Mail = @FirstMailPart + CAST(@MailCount AS NVARCHAR(10)) + '@' + @SecondMailPart;
        END

        -- Start transaction before inserts
        BEGIN TRANSACTION;

        -- Insert on PERSONS
        INSERT INTO PERSONS (
			names,
			surnames,
			identification,
			birth_date
		) VALUES (
			@Names,
			@Surnames,
			@Identification,
			@BirthDate
		);

		-- Getting Person ID
        SET @IdPerson = SCOPE_IDENTITY();

        -- Insert on USERS
        INSERT INTO USERS (
			username,
			password,
			mail,
			session_active,
			status,
			id_person
		) VALUES (
			@Username,
			@Password,
			@Mail,
			@SessionActive,
			@Status,
			@IdPerson
		);

		-- Getting User ID
        SET @IdUser = SCOPE_IDENTITY();

        -- Insert on ROLES_USERS
        CREATE TABLE #IdRoles (id_role INT);

		INSERT INTO #IdRoles (
			id_role
		) SELECT
			CAST(value AS INT) AS id_role
		FROM STRING_SPLIT(@IdRoles, ',')
		WHERE TRY_CAST(value AS INT) IS NOT NULL;

        INSERT INTO ROLES_USERS (
        	id_role,
        	id_user
        ) SELECT
        	id_role,
        	@IdUser AS id_user
        FROM #IdRoles;

        -- Commit the transaction if all inserts succeed
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		DECLARE
			@ErrorMessage NVARCHAR(4000),
			@ErrorNumber INT,
			@ErrorState INT;

        SET @ErrorNumber = ERROR_NUMBER();
		SET @ErrorMessage = ERROR_MESSAGE();
        SET @ErrorState = ERROR_STATE();

        -- Rollback the transaction only if one was started
        IF @@TRANCOUNT > 0
        BEGIN
            -- Eliminar el registro de PERSONS si ya se había insertado
            IF @IdPerson IS NOT NULL
            BEGIN
                DELETE FROM PERSONS WHERE id = @IdPerson;
            END

            ROLLBACK TRANSACTION;
        END

		IF @ErrorNumber = 50001 OR @ErrorNumber = 50002 OR @ErrorNumber = 50003
		BEGIN
			THROW @ErrorNumber, @ErrorMessage, @ErrorState;
		END
		ELSE
		BEGIN
			THROW;
		END
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_PERSON_USER]    Script Date: 16/2/2025 03:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UPDATE_PERSON_USER]
    -- Person columns
	@IdPerson INT,
	@Identification NVARCHAR(10),
	@Names NVARCHAR(60),
    @Surnames NVARCHAR(60),
	@BirthDate DATETIME,

	-- User columns
	@IdUser INT,
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
	@SessionActive BIT,
	@Status NVARCHAR(50),

	-- Role Params
	@IdRoles NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;

    BEGIN TRY
        -- Check if username exists on USERS
        IF EXISTS (SELECT 1 FROM USERS WHERE username = @Username AND id <> @IdUser)
        BEGIN
            THROW 50001, 'El nombre de usuario ya existe en el sistema.', 1;
        END

        -- Start transaction before inserts
        BEGIN TRANSACTION;

        -- Update on PERSONS
		UPDATE PERSONS SET
			identification = @Identification,
			names = @Names,
			surnames = @Surnames,
			birth_date = @BirthDate
		WHERE id = @IdPerson;

        -- Insert on USERS
        IF @Password = ''
        BEGIN
        	UPDATE USERS SET
				username = @Username,
				session_active = @SessionActive,
				status = @Status
			WHERE id = @IdUser;
		END
		ELSE
		BEGIN
			UPDATE USERS SET
				username = @Username,
				password = @Password,
				session_active = @SessionActive,
				status = @Status
			WHERE id = @IdUser;
		END

		-- Insert on ROLES_USERS
		DELETE FROM ROLES_USERS
		WHERE id_user = @IdUser;
		
		CREATE TABLE #IdRoles (id_role INT);

		INSERT INTO #IdRoles (
			id_role
		) SELECT
			CAST(value AS INT) AS id_role
		FROM STRING_SPLIT(@IdRoles, ',')
		WHERE TRY_CAST(value AS INT) IS NOT NULL;

        INSERT INTO ROLES_USERS (
        	id_role,
        	id_user
        ) SELECT
        	id_role,
        	@IdUser AS id_user
        FROM #IdRoles;

        -- Commit the transaction if all inserts succeed
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		DECLARE
			@ErrorMessage NVARCHAR(4000),
			@ErrorNumber INT,
			@ErrorState INT;

        SET @ErrorNumber = ERROR_NUMBER();
		SET @ErrorMessage = ERROR_MESSAGE();
        SET @ErrorState = ERROR_STATE();

        -- Rollback the transaction only if one was started
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

		IF @ErrorNumber = 50001 OR @ErrorNumber = 50002 OR @ErrorNumber = 50003
		BEGIN
			THROW @ErrorNumber, @ErrorMessage, @ErrorState;
		END
		ELSE
		BEGIN
			THROW;
		END
    END CATCH
END;
GO
USE [master]
GO
ALTER DATABASE [prueba_viamatica] SET  READ_WRITE 
GO
