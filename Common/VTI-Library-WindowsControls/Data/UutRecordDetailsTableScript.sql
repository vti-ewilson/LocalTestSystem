USE [VtiData]
GO

/****** Object:  Table [dbo].[UutRecordDetails]    Script Date: 1/28/2021 8:18:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UutRecordDetails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UutRecordID] [bigint] NOT NULL,
	[DateTime] [datetime] NOT NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UutRecordDetails] ADD  CONSTRAINT [DF_UutRecordDetails_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO

ALTER TABLE [dbo].[UutRecordDetails]  WITH CHECK ADD  CONSTRAINT [FK_tblUutRecordDetails_tblUutRecords] FOREIGN KEY([UutRecordID])
REFERENCES [dbo].[UutRecords] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UutRecordDetails] CHECK CONSTRAINT [FK_tblUutRecordDetails_tblUutRecords]
GO

