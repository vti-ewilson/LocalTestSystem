/* Drop foreign key constraint on UutRecords, since */
/* Models table doesn't contain the Default model.  */
ALTER TABLE dbo.UutRecords
	DROP CONSTRAINT FK_UutRecords_Models
GO
