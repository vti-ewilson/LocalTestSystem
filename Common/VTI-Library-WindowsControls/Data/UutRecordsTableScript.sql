USE [VtiData]
GO

/****** Object:  Table [dbo].[UutRecords]    Script Date: 1/28/2021 8:18:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UutRecords](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SerialNo] [nvarchar](50) NOT NULL,
	[ModelNo] [nvarchar](50) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[SystemID] [nvarchar](50) NOT NULL,
	[OpID] [nvarchar](16) NOT NULL,
	[TestType] [nvarchar](25) NOT NULL,
	[TestResult] [nvarchar](50) NOT NULL,
	[TestPort] [nvarchar](25) NULL,
 CONSTRAINT [PK_tblUutRecords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UutRecords] ADD  CONSTRAINT [DF_UutRecords_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO

