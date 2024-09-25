USE [VtiData]
GO

/****** Object:  Table [dbo].[TEST_RESULTS]    Script Date: 04/14/2021 14:47:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TEST_RESULTS](
	[Test_Number] [bigint] IDENTITY(1,1) NOT NULL,
	[Serial_Number] [varchar](50) NOT NULL,
	[Model_Number] [varchar](50) NULL,
	[Date_Time] [datetime] NOT NULL,
	[System_ID] [varchar](50) NULL,
	[Op_ID] [varchar](50) NULL,
	[First_PD_Test] [bit] NULL,
	[First_LD_Test] [bit] NULL,
	[Flow_Rate] [float] NULL,
	[Reject_Criteria] [float] NULL,
	[Test_Result] [varchar](255) NULL,
	[Test_Code] [varchar](50) NULL,
 CONSTRAINT [PK_TEST_RESULTS] PRIMARY KEY CLUSTERED 
(
	[Test_Number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

