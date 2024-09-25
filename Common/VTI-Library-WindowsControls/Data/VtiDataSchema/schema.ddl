SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Models]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Models](
	[ModelNo] [nvarchar](50) NOT NULL,
	[LastModifiedBy] [nvarchar](50) NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_Models] PRIMARY KEY CLUSTERED 
(
	[ModelNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Models]') AND name = N'IX_tblModels_LastModified')
CREATE NONCLUSTERED INDEX [IX_tblModels_LastModified] ON [dbo].[Models] 
(
	[LastModified] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Models]') AND name = N'IX_tblModels_LastModifiedBy')
CREATE NONCLUSTERED INDEX [IX_tblModels_LastModifiedBy] ON [dbo].[Models] 
(
	[LastModifiedBy] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Groups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Groups](
	[GroupID] [nvarchar](10) NOT NULL,
	[IsLocked] [bit] NOT NULL CONSTRAINT [DF_tblGroups_IsLocked]  DEFAULT ((1)),
 CONSTRAINT [PK_tblGroups] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SchemaChanges]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SchemaChanges](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[major] [int] NULL,
	[minor] [int] NULL,
	[release] [int] NULL,
	[script_name] [varchar](255) NULL,
	[applied] [datetime] NULL,
 CONSTRAINT [PK_schema_information] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UutDefects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UutDefects](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UutRecordID] [bigint] NOT NULL,
	[Defect] [nvarchar](25) NOT NULL,
	[Category] [nvarchar](25) NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblUutDefects] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutDefects]') AND name = N'IX_tblUutDefects_Category')
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_Category] ON [dbo].[UutDefects] 
(
	[Category] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutDefects]') AND name = N'IX_tblUutDefects_Defect')
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_Defect] ON [dbo].[UutDefects] 
(
	[Defect] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutDefects]') AND name = N'IX_tblUutDefects_UutRecordID')
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_UutRecordID] ON [dbo].[UutDefects] 
(
	[UutRecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutDefects]') AND name = N'IX_tblUutDefects_UutRecordID_Defect')
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_UutRecordID_Defect] ON [dbo].[UutDefects] 
(
	[UutRecordID] ASC,
	[Defect] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UutRecordDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UutRecordDetails](
	[ID] [bigint] NOT NULL,
	[UutRecordID] [bigint] NOT NULL,
	[DateTime] [datetime] NOT NULL CONSTRAINT [DF_UutRecordDetails_DateTime]  DEFAULT (getdate()),
	[Test] [nvarchar](50) NOT NULL,
	[Result] [nvarchar](50) NOT NULL,
	[ValueName] [nvarchar](50) NULL,
	[Value] [float] NULL,
	[MinSetpointName] [nvarchar](50) NULL,
	[MinSetpoint] [float] NULL,
	[MaxSetpointName] [nvarchar](50) NULL,
	[MaxSetpoint] [float] NULL,
	[Units] [nvarchar](50) NULL,
	[ElapsedTime] [float] NULL,
 CONSTRAINT [PK_tblUutRecordDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecordDetails]') AND name = N'IX_tblUutRecordDetails_UutRecordID_DateTime')
CREATE NONCLUSTERED INDEX [IX_tblUutRecordDetails_UutRecordID_DateTime] ON [dbo].[UutRecordDetails] 
(
	[UutRecordID] ASC,
	[DateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecordDetails]') AND name = N'IX_tblUutRecordDetails_UutRecordID_ID')
CREATE NONCLUSTERED INDEX [IX_tblUutRecordDetails_UutRecordID_ID] ON [dbo].[UutRecordDetails] 
(
	[UutRecordID] ASC,
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecordDetails]') AND name = N'IX_tblUutRecordDetails_UutRecordID_TestName')
CREATE NONCLUSTERED INDEX [IX_tblUutRecordDetails_UutRecordID_TestName] ON [dbo].[UutRecordDetails] 
(
	[UutRecordID] ASC,
	[Test] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ModelParameters]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ModelParameters](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModelNo] [nvarchar](50) NULL,
	[ParameterID] [nvarchar](50) NULL,
	[ProcessValue] [nvarchar](255) NULL,
	[LastModifiedBy] [nvarchar](50) NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_tblModelParameters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ModelParameters]') AND name = N'IX_tblModelParameters_LastModified')
CREATE NONCLUSTERED INDEX [IX_tblModelParameters_LastModified] ON [dbo].[ModelParameters] 
(
	[LastModified] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ModelParameters]') AND name = N'IX_tblModelParameters_ModelNo')
CREATE NONCLUSTERED INDEX [IX_tblModelParameters_ModelNo] ON [dbo].[ModelParameters] 
(
	[ModelNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ModelParameters]') AND name = N'IX_tblModelParameters_ModelNo_ParameterID')
CREATE NONCLUSTERED INDEX [IX_tblModelParameters_ModelNo_ParameterID] ON [dbo].[ModelParameters] 
(
	[ModelNo] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UutRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UutRecords](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SerialNo] [nvarchar](50) NOT NULL,
	[ModelNo] [nvarchar](50) NOT NULL,
	[DateTime] [datetime] NOT NULL CONSTRAINT [DF_UutRecords_DateTime]  DEFAULT (getdate()),
	[SystemID] [nvarchar](50) NOT NULL,
	[OpID] [nvarchar](16) NOT NULL,
	[TestType] [nvarchar](25) NOT NULL,
	[TestResult] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tblUutRecords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecords]') AND name = N'IX_tblUutRecords_DateTime')
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_DateTime] ON [dbo].[UutRecords] 
(
	[DateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecords]') AND name = N'IX_tblUutRecords_ModelNo')
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_ModelNo] ON [dbo].[UutRecords] 
(
	[ModelNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecords]') AND name = N'IX_tblUutRecords_SerialNo')
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_SerialNo] ON [dbo].[UutRecords] 
(
	[SerialNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UutRecords]') AND name = N'IX_tblUutRecords_SerialNo_TestResult')
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_SerialNo_TestResult] ON [dbo].[UutRecords] 
(
	[SerialNo] ASC,
	[TestResult] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupCommands]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GroupCommands](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupID] [nvarchar](10) NOT NULL,
	[CommandID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tblGroupCommands] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[GroupCommands]') AND name = N'IX_tblGroupCommands_GroupID_CommandID')
CREATE NONCLUSTERED INDEX [IX_tblGroupCommands_GroupID_CommandID] ON [dbo].[GroupCommands] 
(
	[GroupID] ASC,
	[CommandID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OpID] [nvarchar](16) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[GroupID] [nvarchar](10) NOT NULL,
	[IsLocked] [bit] NOT NULL CONSTRAINT [DF_tblUsers_IsLocked]  DEFAULT ((0)),
 CONSTRAINT [PK_tblUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'IX_tblUsers_GroupID')
CREATE NONCLUSTERED INDEX [IX_tblUsers_GroupID] ON [dbo].[Users] 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'IX_tblUsers_OpID')
CREATE UNIQUE NONCLUSTERED INDEX [IX_tblUsers_OpID] ON [dbo].[Users] 
(
	[OpID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'IX_tblUsers_Password')
CREATE NONCLUSTERED INDEX [IX_tblUsers_Password] ON [dbo].[Users] 
(
	[Password] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblUutDefects_tblUutRecords]') AND parent_object_id = OBJECT_ID(N'[dbo].[UutDefects]'))
ALTER TABLE [dbo].[UutDefects]  WITH CHECK ADD  CONSTRAINT [FK_tblUutDefects_tblUutRecords] FOREIGN KEY([UutRecordID])
REFERENCES [dbo].[UutRecords] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UutDefects] CHECK CONSTRAINT [FK_tblUutDefects_tblUutRecords]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblUutRecordDetails_tblUutRecords]') AND parent_object_id = OBJECT_ID(N'[dbo].[UutRecordDetails]'))
ALTER TABLE [dbo].[UutRecordDetails]  WITH CHECK ADD  CONSTRAINT [FK_tblUutRecordDetails_tblUutRecords] FOREIGN KEY([UutRecordID])
REFERENCES [dbo].[UutRecords] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UutRecordDetails] CHECK CONSTRAINT [FK_tblUutRecordDetails_tblUutRecords]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ModelParameters_Models]') AND parent_object_id = OBJECT_ID(N'[dbo].[ModelParameters]'))
ALTER TABLE [dbo].[ModelParameters]  WITH CHECK ADD  CONSTRAINT [FK_ModelParameters_Models] FOREIGN KEY([ModelNo])
REFERENCES [dbo].[Models] ([ModelNo])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ModelParameters] CHECK CONSTRAINT [FK_ModelParameters_Models]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UutRecords_Models]') AND parent_object_id = OBJECT_ID(N'[dbo].[UutRecords]'))
ALTER TABLE [dbo].[UutRecords]  WITH CHECK ADD  CONSTRAINT [FK_UutRecords_Models] FOREIGN KEY([ModelNo])
REFERENCES [dbo].[Models] ([ModelNo])
GO
ALTER TABLE [dbo].[UutRecords] CHECK CONSTRAINT [FK_UutRecords_Models]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UutRecords_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UutRecords]'))
ALTER TABLE [dbo].[UutRecords]  WITH CHECK ADD  CONSTRAINT [FK_UutRecords_Users] FOREIGN KEY([OpID])
REFERENCES [dbo].[Users] ([OpID])
GO
ALTER TABLE [dbo].[UutRecords] CHECK CONSTRAINT [FK_UutRecords_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblGroupCommands_tblGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupCommands]'))
ALTER TABLE [dbo].[GroupCommands]  WITH CHECK ADD  CONSTRAINT [FK_tblGroupCommands_tblGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[GroupCommands] CHECK CONSTRAINT [FK_tblGroupCommands_tblGroups]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblUsers_tblGroups]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_tblUsers_tblGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_tblUsers_tblGroups]
GO

/*================================================================================*/
/* Set initial schema revision number.                                            */
/*================================================================================*/

INSERT INTO [SchemaChanges]
       ([major]
       ,[minor]
       ,[release]
       ,[script_name]
       ,[applied])
VALUES
       (1
       ,0
       ,0
       ,'baseline'
       ,GETDATE())
GO

