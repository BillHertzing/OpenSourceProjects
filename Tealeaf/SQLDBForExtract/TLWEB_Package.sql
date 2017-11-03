/*
	Target database:	TLWEB (configurable)
	Target instance:	(any)
	Generated date:		1/31/2013 7:56:34 AM
	Generated on:		NCAT021
	ReadyRoll version:	1.4.0.0
	Migrations pending:	(variable)

	IMPORTANT! "SQLCMD Mode" must be activated prior to execution (under the Query menu in SSMS).

	BEFORE EXECUTING THIS SCRIPT, WE STRONGLY RECOMMEND YOU TAKE A BACKUP OF YOUR DATABASE.

	This SQLCMD script is designed to be executed through MSBuild (via the .dbproj Deploy target) however 
	it can also be run manually using SQL Management Studio. 

	It was generared by the ReadyRoll build task and contains logic to deploy the database, ensuring that 
	each of the contained "Deploy-Once" scripts is executed a single time only in alphabetical (filename) 
	order. If any errors occur within those scripts, the deployment will be aborted and the transaction
	rolled-back.

	NOTE: Automatic transaction management is provided for Deploy-Once migrations, so you don't need to
		  add any special BEGIN TRAN/COMMIT/ROLLBACK logic in those script files. 
		  However if you require transaction handling in your Pre/Post-Deployment scripts, you will
		  need to add this logic to the source .sql files yourself.
*/

----====================================================================================================================
---- SQLCMD Variables
---- This script is designed to be called by SQLCMD.EXE with variables specified on the command line.
---- However you can also run it in SQL Management Studio by uncommenting this section (CTRL+K, CTRL+U).
--:setvar DatabaseName "TLWEB"
----====================================================================================================================

:on error exit -- Instructs SQLCMD to abort execution as soon as an erroneous batch is encountered

GO

SET IMPLICIT_TRANSACTIONS, NUMERIC_ROUNDABORT OFF;
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, NOCOUNT, QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON; -- Abort the current batch immediately if a statement raises a run-time error and rollback any open transaction(s)

IF N'$(DatabaseName)' = N'$' + N'(DatabaseName)' -- Is SQLCMD mode enabled within the execution context (eg. SSMS)
	BEGIN
		IF IS_SRVROLEMEMBER(N'sysadmin') = 1
			BEGIN -- User is sysadmin; abort execution by disconnect the script from the database server
				RAISERROR(N'This script must be run in SQLCMD Mode (under the Query menu in SSMS). Aborting connection to suppress subsequent errors.', 20, 127, N'UNKNOWN') WITH LOG;
			END
		ELSE
			BEGIN -- User is not sysadmin; abort execution by switching off statement execution (script will continue to the end without performing any actual deployment work)
				RAISERROR(N'This script must be run in SQLCMD Mode (under the Query menu in SSMS). Script execution has been halted.', 16, 127, N'UNKNOWN') WITH NOWAIT;
			END		
	END		
GO
IF @@ERROR != 0
	BEGIN
		SET NOEXEC ON; -- SQLCMD is NOT enabled so prevent any further statements from executing
	END
GO
-- Beyond this point, no further explicit error handling is required because it can be assumed that SQLCMD mode is enabled
IF DB_NAME() != 'master'
	BEGIN
		USE [master];
	END
GO








------------------------------------------------------------------------------------------------------------------------
------------------------------------------       PRE-DEPLOYMENT SCRIPTS       ------------------------------------------
------------------------------------------------------------------------------------------------------------------------

SET IMPLICIT_TRANSACTIONS, NUMERIC_ROUNDABORT OFF;
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, NOCOUNT, QUOTED_IDENTIFIER ON;

--------------------------------- BEGIN PRE-DEPLOYMENT SCRIPT: "01_Create_Database.sql" ----------------------------------
GO
IF (DB_ID(N'$(DatabaseName)') IS NULL) 
BEGIN
	PRINT N'Creating $(DatabaseName)...';
END
GO
IF (DB_ID(N'$(DatabaseName)') IS NULL) 
BEGIN
	CREATE DATABASE [$(DatabaseName)]; -- MODIFY THIS STATEMENT TO SPECIFY A COLLATION FOR YOUR DATABASE
END

GO
---------------------------------- END PRE-DEPLOYMENT SCRIPT: "01_Create_Database.sql" -----------------------------------









------------------------------------------------------------------------------------------------------------------------
------------------------------------------         DEPLOY-ONCE SCRIPTS        ------------------------------------------
------------------------------------------------------------------------------------------------------------------------

SET IMPLICIT_TRANSACTIONS, NUMERIC_ROUNDABORT OFF;

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, NOCOUNT, QUOTED_IDENTIFIER ON;


GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

BEGIN TRANSACTION;


GO
IF DB_NAME() != '$(DatabaseName)'
    USE [$(DatabaseName)];


GO
IF (NOT EXISTS (SELECT *
                FROM   sys.objects
                WHERE  [object_id] = OBJECT_ID(N'[dbo].[__MigrationLog]')
                       AND [type] = 'U'))
    BEGIN
        CREATE TABLE [dbo].[__MigrationLog] (
            [migration_id]    UNIQUEIDENTIFIER NOT NULL,
            [script_checksum] NVARCHAR (64)    NOT NULL,
            [script_filename] NVARCHAR (255)   NOT NULL,
            [complete_dt]     DATETIME2        NOT NULL,
            [applied_by]      NVARCHAR (100)   NOT NULL,
            [deployed]        BIT              CONSTRAINT [DF___MigrationLog_deployed] DEFAULT (1) NOT NULL CONSTRAINT [PK___MigrationLog] PRIMARY KEY CLUSTERED ([migration_id], [script_checksum])
        );
        CREATE NONCLUSTERED INDEX [IX___MigrationLog_CompleteDt]
            ON [dbo].[__MigrationLog]([complete_dt]);
    END


GO
IF DB_NAME() != '$(DatabaseName)'
    USE [$(DatabaseName)];


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        PRINT '

***** EXECUTING DEPLOY-ONCE SCRIPT ''0001_20130107-1619_whertzing.sql'', ID: {ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4} *****';
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('-- <Migration ID="ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4" />



Print ''Create Table [dbo].[TLWEB_APPDATA_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_APPDATA_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[NAME]                  [nvarchar](256) NULL,
		[VALUE]                 [nvarchar](512) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_AP__0C1B4A4DC447FA3C] to [dbo].[TLWEB_APPDATA_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_APPDATA_8X]
	ADD
	CONSTRAINT [PK__TLWEB_AP__0C1B4A4DC447FA3C]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_APP] on [dbo].[TLWEB_APPDATA_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_APP]
	ON [dbo].[TLWEB_APPDATA_8X] ([SESSION_KEY], [HIT_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_APPDATA_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_ATTRIBUTE_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_ATTRIBUTE_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[ID]                    [int] NULL,
		[NAME]                  [nvarchar](256) NULL,
		[VALUE]                 [nvarchar](512) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_AT__0C1B4A4DFAEF25A3] to [dbo].[TLWEB_ATTRIBUTE_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_ATTRIBUTE_8X]
	ADD
	CONSTRAINT [PK__TLWEB_AT__0C1B4A4DFAEF25A3]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_ATT] on [dbo].[TLWEB_ATTRIBUTE_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_ATT]
	ON [dbo].[TLWEB_ATTRIBUTE_8X] ([SESSION_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_ATTRIBUTE_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_COOKIE_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_COOKIE_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[NAME]                  [nvarchar](128) NULL,
		[VALUE]                 [nvarchar](256) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_CO__0C1B4A4D904E06F4] to [dbo].[TLWEB_COOKIE_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_COOKIE_8X]
	ADD
	CONSTRAINT [PK__TLWEB_CO__0C1B4A4D904E06F4]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_COK] on [dbo].[TLWEB_COOKIE_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_COK]
	ON [dbo].[TLWEB_COOKIE_8X] ([SESSION_KEY], [HIT_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_COOKIE_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_DIMENSIONS_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_DIMENSIONS_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[EVENT_ID]              [bigint] NOT NULL,
		[DIM_GRP_ID]            [bigint] NOT NULL,
		[FACT_ID]               [bigint] NOT NULL,
		[FACT_VALUE]            [nvarchar](256) NULL,
		[DIMENSION_1]           [nvarchar](256) NULL,
		[DIMENSION_2]           [nvarchar](256) NULL,
		[DIMENSION_3]           [nvarchar](256) NULL,
		[DIMENSION_4]           [nvarchar](256) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_DI__0C1B4A4D439B2032] to [dbo].[TLWEB_DIMENSIONS_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [PK__TLWEB_DI__0C1B4A4D439B2032]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Default Constraint [DF__TLWEB_DIM__DIM_G__1DE57479] to [dbo].[TLWEB_DIMENSIONS_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [DF__TLWEB_DIM__DIM_G__1DE57479]
	DEFAULT ((0)) FOR [DIM_GRP_ID]
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Default Constraint [DF__TLWEB_DIM__EVENT__1CF15040] to [dbo].[TLWEB_DIMENSIONS_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [DF__TLWEB_DIM__EVENT__1CF15040]
	DEFAULT ((0)) FOR [EVENT_ID]
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Default Constraint [DF__TLWEB_DIM__FACT___1ED998B2] to [dbo].[TLWEB_DIMENSIONS_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [DF__TLWEB_DIM__FACT___1ED998B2]
	DEFAULT ((0)) FOR [FACT_ID]
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_EFC] on [dbo].[TLWEB_DIMENSIONS_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_EFC]
	ON [dbo].[TLWEB_DIMENSIONS_8X] ([SESSION_KEY], [HIT_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_EVENT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_EVENT_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[EVENT_ID]              [bigint] NOT NULL,
		[TEXT_FOUND]            [nvarchar](512) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_EV__0C1B4A4D96984469] to [dbo].[TLWEB_EVENT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_EVENT_8X]
	ADD
	CONSTRAINT [PK__TLWEB_EV__0C1B4A4D96984469]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Default Constraint [DF__TLWEB_EVE__EVENT__1A14E395] to [dbo].[TLWEB_EVENT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_EVENT_8X]
	ADD
	CONSTRAINT [DF__TLWEB_EVE__EVENT__1A14E395]
	DEFAULT ((0)) FOR [EVENT_ID]
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_EV8] on [dbo].[TLWEB_EVENT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_EV8]
	ON [dbo].[TLWEB_EVENT_8X] ([SESSION_KEY], [HIT_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_EVENT_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_HIT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_HIT_8X] (
		[TAB_KEY]                 [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]             [varchar](32) NULL,
		[HIT_KEY]                 [varchar](32) NULL,
		[TLTHID]                  [varchar](32) NULL,
		[HTTP_SECURE]             [varchar](1) NULL,
		[HIT_DURATION]            [bigint] NULL,
		[HIT_NUMBER]              [bigint] NULL,
		[REFERER]                 [nvarchar](512) NULL,
		[REQ_TIMESTAMP]           [datetime] NULL,
		[REQ_END_TIMESTAMP]       [datetime] NULL,
		[RSP_START_TIMESTAMP]     [datetime] NULL,
		[RSP_ACK_TIMESTAMP]       [datetime] NULL,
		[RSP_TIMESTAMP]           [datetime] NULL,
		[REQ_SIZE]                [bigint] NULL,
		[RSP_SIZE]                [bigint] NULL,
		[REQ_CANCELLED]           [varchar](1) NULL,
		[RSP_TTFB]                [bigint] NULL,
		[RSP_TTLB]                [bigint] NULL,
		[RSP_TTLA]                [bigint] NULL,
		[CON_SPEED]               [bigint] NULL,
		[CON_TYPE]                [nvarchar](20) NULL,
		[QUERY_STRING]            [nvarchar](512) NULL,
		[HTTP_STATUS]             [bigint] NULL,
		[URL]                     [nvarchar](256) NULL,
		[REQ_METHOD]              [char](20) NULL,
		[RSP_TYPE]                [char](64) NULL,
		[HOST_NAME]               [varchar](100) NULL,
		[HOST_IP]                 [varchar](40) NULL,
		[WS_GEN]                  [bigint] NULL,
		[NT_GEN]                  [bigint] NULL,
		[RT_GEN]                  [bigint] NULL,
		[ACCEPT_LANG]             [nvarchar](64) NULL,
		[PAGE_RENDER]             [bigint] NULL,
		[PAGE_DWELL]              [bigint] NULL,
		[SESSION_TIMESTAMP]       [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_HI__0C1B4A4DB4BA2027] to [dbo].[TLWEB_HIT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_HIT_8X]
	ADD
	CONSTRAINT [PK__TLWEB_HI__0C1B4A4DB4BA2027]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_HIT] on [dbo].[TLWEB_HIT_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_HIT]
	ON [dbo].[TLWEB_HIT_8X] ([SESSION_KEY], [HIT_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_HIT_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_SESSION_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_SESSION_8X] (
		[TAB_KEY]                 [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]             [varchar](32) NULL,
		[REMOTE_ADDRESS]          [varchar](40) NULL,
		[CANISTER_LSSN]           [varchar](256) NULL,
		[CANISTER_SESSION_ID]     [bigint] NULL,
		[TLTSID]                  [varchar](32) NULL,
		[TLTUID]                  [varchar](32) NULL,
		[TLTVID]                  [varchar](32) NULL,
		[CANISTER_SERVER]         [varchar](64) NULL,
		[SESSION_TIMESTAMP]       [datetime] NULL,
		[SESSION_DURATION]        [bigint] NULL,
		[HIT_COUNT]               [bigint] NULL,
		[HTTP_USER_AGENT]         [nvarchar](256) NULL,
		[EXTRACTID]               [bigint] NULL,
		[TEALEAF_REPLAY]          [varchar](128) NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_SE__0C1B4A4D347F0A32] to [dbo].[TLWEB_SESSION_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_SESSION_8X]
	ADD
	CONSTRAINT [PK__TLWEB_SE__0C1B4A4D347F0A32]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_SES] on [dbo].[TLWEB_SESSION_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_SES]
	ON [dbo].[TLWEB_SESSION_8X] ([SESSION_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_SESSION_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Table [dbo].[TLWEB_URLFIELD_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE TABLE [dbo].[TLWEB_URLFIELD_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[NAME]                  [nvarchar](256) NULL,
		[VALUE]                 [nvarchar](1024) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Add Primary Key [PK__TLWEB_UR__0C1B4A4D4B4D027D] to [dbo].[TLWEB_URLFIELD_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('ALTER TABLE [dbo].[TLWEB_URLFIELD_8X]
	ADD
	CONSTRAINT [PK__TLWEB_UR__0C1B4A4D4B4D027D]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

Print ''Create Index [IDX_UFD] on [dbo].[TLWEB_URLFIELD_8X]''
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('CREATE NONCLUSTERED INDEX [IDX_UFD]
	ON [dbo].[TLWEB_URLFIELD_8X] ([SESSION_KEY], [HIT_KEY])
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        EXECUTE ('

ALTER TABLE [dbo].[TLWEB_URLFIELD_8X] SET (LOCK_ESCALATION = TABLE)
');
    END


GO
IF NOT EXISTS (SELECT *
               FROM   [$(DatabaseName)].[dbo].[__MigrationLog]
               WHERE  [migration_id] = CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER))
    BEGIN
        INSERT  [$(DatabaseName)].[dbo].[__MigrationLog] ([migration_id], [script_checksum], [script_filename], [complete_dt], [applied_by], [deployed])
        VALUES                                          (CAST ('ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4' AS UNIQUEIDENTIFIER), 'C5598680E449E88B3AA96DFF08F525CFF1908D8081B3FB1FA827D0BDA5D1C920', '0001_20130107-1619_whertzing.sql', SYSDATETIME(), SYSTEM_USER, 1);
        PRINT '***** FINISHED EXECUTING DEPLOY-ONCE SCRIPT ''0001_20130107-1619_whertzing.sql'', ID: {ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4} *****
';
    END


GO
COMMIT TRANSACTION;

GO







------------------------------------------------------------------------------------------------------------------------
------------------------------------------       POST-DEPLOYMENT SCRIPTS      ------------------------------------------
------------------------------------------------------------------------------------------------------------------------


SET IMPLICIT_TRANSACTIONS, NUMERIC_ROUNDABORT OFF;
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, NOCOUNT, QUOTED_IDENTIFIER ON;

IF DB_NAME() != '$(DatabaseName)'
    USE [$(DatabaseName)];

------------------------------ BEGIN POST-DEPLOYMENT SCRIPT: "01_Finalize_Deployment.sql" --------------------------------
GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
GO
------------------------------- END POST-DEPLOYMENT SCRIPT: "01_Finalize_Deployment.sql" ---------------------------------






SET NOEXEC OFF; -- Resume statement execution if an error occurred within the script pre-amble
