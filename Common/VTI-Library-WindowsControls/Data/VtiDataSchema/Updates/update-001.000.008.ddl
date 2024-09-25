/* Drop the Users foreign key constraint on UutRecords, to allow               */
/* some applications to prompt for a username seperate from the login account  */
ALTER TABLE dbo.UutRecords
	DROP CONSTRAINT FK_UutRecords_Users
GO
