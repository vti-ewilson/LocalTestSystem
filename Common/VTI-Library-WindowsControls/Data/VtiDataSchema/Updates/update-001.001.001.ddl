/*================================================================================*/
/* Add ModelXRef (Model Cross-Reference) table                                    */
/* This table will be used when loading a model to cross reference the            */
/* characters scanned by barcode to the actual system model number.               */
/*================================================================================*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE dbo.ModelXRef
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	ModelNo nvarchar(50) NOT NULL,
	ScannedChars nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ModelXRef ADD CONSTRAINT
	PK_ModelXRef PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_ModelXRef_ModelNo ON dbo.ModelXRef
	(
	ModelNo
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_ModelXRef_ScannedChars ON dbo.ModelXRef
	(
	ScannedChars
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
