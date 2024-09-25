USE [VtiData]
GO

/****** Object:  Table [dbo].[ManualCmdExecLog]    Script Date: 1/25/2021 2:15:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ManualCmdExecLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[OpID] [nvarchar](100) NOT NULL,
	[OverrideOpID] [nvarchar](100) NULL,
	[SystemID] [nvarchar](100) NOT NULL,
	[ManualCommand] [nvarchar](200) NOT NULL
) ON [PRIMARY]
GO

