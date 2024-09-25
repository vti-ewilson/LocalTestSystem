/* Converts ID field in UutRecordDetails to Identity field */

ALTER TABLE dbo.UutRecordDetails
	DROP CONSTRAINT FK_tblUutRecordDetails_tblUutRecords
GO
ALTER TABLE dbo.UutRecordDetails
	DROP CONSTRAINT DF_UutRecordDetails_DateTime
GO
CREATE TABLE dbo.Tmp_UutRecordDetails
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	UutRecordID bigint NOT NULL,
	DateTime datetime NOT NULL,
	Test nvarchar(50) NOT NULL,
	Result nvarchar(50) NOT NULL,
	ValueName nvarchar(50) NULL,
	Value float(53) NULL,
	MinSetpointName nvarchar(50) NULL,
	MinSetpoint float(53) NULL,
	MaxSetpointName nvarchar(50) NULL,
	MaxSetpoint float(53) NULL,
	Units nvarchar(50) NULL,
	ElapsedTime float(53) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_UutRecordDetails ADD CONSTRAINT
	DF_UutRecordDetails_DateTime DEFAULT (getdate()) FOR DateTime
GO
SET IDENTITY_INSERT dbo.Tmp_UutRecordDetails ON
GO
IF EXISTS(SELECT * FROM dbo.UutRecordDetails)
	 EXEC('INSERT INTO dbo.Tmp_UutRecordDetails (ID, UutRecordID, DateTime, Test, Result, ValueName, Value, MinSetpointName, MinSetpoint, MaxSetpointName, MaxSetpoint, Units, ElapsedTime)
		SELECT ID, UutRecordID, DateTime, Test, Result, ValueName, Value, MinSetpointName, MinSetpoint, MaxSetpointName, MaxSetpoint, Units, ElapsedTime FROM dbo.UutRecordDetails WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_UutRecordDetails OFF
GO
DROP TABLE dbo.UutRecordDetails
GO
EXECUTE sp_rename N'dbo.Tmp_UutRecordDetails', N'UutRecordDetails', 'OBJECT' 
GO
ALTER TABLE dbo.UutRecordDetails ADD CONSTRAINT
	PK_tblUutRecordDetails PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblUutRecordDetails_UutRecordID_ID ON dbo.UutRecordDetails
	(
	UutRecordID,
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblUutRecordDetails_UutRecordID_TestName ON dbo.UutRecordDetails
	(
	UutRecordID,
	Test
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblUutRecordDetails_UutRecordID_DateTime ON dbo.UutRecordDetails
	(
	UutRecordID,
	DateTime
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.UutRecordDetails ADD CONSTRAINT
	FK_tblUutRecordDetails_tblUutRecords FOREIGN KEY
	(
	UutRecordID
	) REFERENCES dbo.UutRecords
	(
	ID
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 
	
GO
