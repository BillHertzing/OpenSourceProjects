/*
	Target database:	TLWEB
	Target instance:	NCAT021
	Generated date:		1/31/2013 7:56:36 AM
	Generated on:		NCAT021
	ReadyRoll version:	1.4.0.0
	Migrations pending:	0

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

:setvar DatabaseName "TLWEB"
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

-- As this script has been generated for a specific server instance/database combination, stop execution if there is a mismatch
IF (@@SERVERNAME != 'NCAT021' OR '$(DatabaseName)' != 'TLWEB')
BEGIN
	RAISERROR(N'This script should only be executed on the following server/instance: [NCAT021] (Database: [TLWEB]). Halting deployment.', 16, 127, N'UNKNOWN') WITH NOWAIT;
	RETURN;
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

PRINT 'No migrations pending deployment';


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
