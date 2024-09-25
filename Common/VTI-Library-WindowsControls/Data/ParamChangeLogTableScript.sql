USE [VtiData]
GO

/****** Object:  Table [dbo].[ParamChangeLog]    Script Date: 11/25/2020 12:12:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ParamChangeLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[OpID] [nvarchar](100) NOT NULL,
	[OverrideOpID] [nvarchar](100) NULL,
	[SystemID] [nvarchar](100) NOT NULL,
	[ParameterSectionName] [nvarchar](200) NOT NULL,
	[ParameterName] [nvarchar](200) NOT NULL,
	[OldValue] [nvarchar](200) NOT NULL,
	[NewValue] [nvarchar](200) NOT NULL
) ON [PRIMARY]
GO