/* Adds TestPort field to the UutRecords table */

ALTER TABLE dbo.UutRecords ADD
	TestPort nvarchar(25) NULL
GO
