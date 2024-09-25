USE [VtiData]
GO

/****** Object:  Table [dbo].[Models]    Script Date: 2/6/2023 9:52:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Models](
	[ModelNo] [nvarchar](50) NOT NULL,
	[LastModifiedBy] [nvarchar](50) NULL,
	[LastModified] [datetime] NULL,
	[SystemType] [nvarchar](200) NULL,
 CONSTRAINT [uq_Models] UNIQUE NONCLUSTERED 
(
	[ModelNo] ASC,
	[SystemType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO