﻿IF (DB_ID(N'$(DatabaseName)') IS NULL) 
BEGIN
	PRINT N'Creating $(DatabaseName)...';
END
GO
IF (DB_ID(N'$(DatabaseName)') IS NULL) 
BEGIN
	CREATE DATABASE [$(DatabaseName)]; -- MODIFY THIS STATEMENT TO SPECIFY A COLLATION FOR YOUR DATABASE
END
