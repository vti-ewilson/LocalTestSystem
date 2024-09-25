USE [master]
GO
/****** Object:  Database [VtiData]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE DATABASE [VtiData]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VtiData', FILENAME = N'C:\VTI PC\Databases\VtiData.mdf' , SIZE = 4352KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'VtiData_log', FILENAME = N'C:\VTI PC\Databases\VtiData_log.ldf' , SIZE = 10176KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [VtiData] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VtiData].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VtiData] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VtiData] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VtiData] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VtiData] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VtiData] SET ARITHABORT OFF 
GO
ALTER DATABASE [VtiData] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [VtiData] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VtiData] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VtiData] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VtiData] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VtiData] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VtiData] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VtiData] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VtiData] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VtiData] SET  DISABLE_BROKER 
GO
ALTER DATABASE [VtiData] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VtiData] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VtiData] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VtiData] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VtiData] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VtiData] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VtiData] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VtiData] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [VtiData] SET  MULTI_USER 
GO
ALTER DATABASE [VtiData] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VtiData] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VtiData] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VtiData] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [VtiData] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [VtiData] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [VtiData] SET QUERY_STORE = OFF
GO
USE [VtiData]
GO
/****** Object:  User [vtiuser]    Script Date: 3/28/2024 11:00:04 AM ******/
CREATE USER [vtiuser] FOR LOGIN [vtiuser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[GroupCommands]    Script Date: 3/28/2024 11:00:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupCommands](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupID] [nvarchar](10) NOT NULL,
	[CommandID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tblGroupCommands] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupID] [nvarchar](10) NOT NULL,
	[IsLocked] [bit] NOT NULL,
 CONSTRAINT [PK_tblGroups] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ManualCmdExecLog]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManualCmdExecLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[OpID] [nvarchar](200) NOT NULL,
	[OverrideOpID] [nvarchar](200) NULL,
	[SystemID] [nvarchar](100) NOT NULL,
	[ManualCommand] [nvarchar](200) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelParameters]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelParameters](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModelNo] [nvarchar](50) NULL,
	[ParameterID] [nvarchar](50) NULL,
	[ProcessValue] [nvarchar](255) NULL,
	[LastModifiedBy] [nvarchar](200) NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_tblModelParameters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Models]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Models](
	[ModelNo] [nvarchar](50) NOT NULL,
	[LastModifiedBy] [nvarchar](200) NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_Models] PRIMARY KEY CLUSTERED 
(
	[ModelNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelXRef]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelXRef](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModelNo] [nvarchar](50) NOT NULL,
	[ScannedChars] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ModelXRef] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParamChangeLog]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParamChangeLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[OpID] [nvarchar](200) NOT NULL,
	[OverrideOpID] [nvarchar](200) NULL,
	[SystemID] [nvarchar](100) NOT NULL,
	[ParameterSectionName] [nvarchar](200) NOT NULL,
	[ParameterName] [nvarchar](200) NOT NULL,
	[OldValue] [nvarchar](200) NOT NULL,
	[NewValue] [nvarchar](200) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemaChanges]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OpID] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[GroupID] [nvarchar](10) NOT NULL,
	[IsLocked] [bit] NOT NULL,
 CONSTRAINT [PK_tblUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UutDefects]    Script Date: 1/26/2021 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UutDefects](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UutRecordID] [bigint] NOT NULL,
	[Defect] [nvarchar](25) NOT NULL,
	[Category] [nvarchar](25) NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblUutDefects] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UutRecordDetails]    Script Date: 1/26/2021 9:20:16 AM ******/
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
/****** Object:  Table [dbo].[UutRecords]    Script Date: 1/26/2021 9:20:16 AM ******/
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
	[OpID] [nvarchar](200) NOT NULL,
	[TestType] [nvarchar](25) NOT NULL,
	[TestResult] [nvarchar](200) NOT NULL,
	[TestPort] [nvarchar](25) NULL,
 CONSTRAINT [PK_tblUutRecords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP01', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP02', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP03', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP04', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP05', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP06', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP07', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP08', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP09', 0)
INSERT [dbo].[Groups] ([GroupID], [IsLocked]) VALUES (N'GROUP10', 0)
GO
SET IDENTITY_INSERT [dbo].[SchemaChanges] ON 

INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (1, 1, 0, 0, N'baseline', CAST(N'2015-08-24T07:18:26.753' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (2, 1, 0, 1, N'update-001.000.001.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (3, 1, 0, 2, N'update-001.000.002.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (4, 1, 0, 3, N'update-001.000.003.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (5, 1, 0, 4, N'update-001.000.004.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (6, 1, 0, 5, N'update-001.000.005.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (7, 1, 0, 6, N'update-001.000.006.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (8, 1, 0, 7, N'update-001.000.007.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (9, 1, 0, 8, N'update-001.000.008.ddl', CAST(N'2015-08-24T07:18:26.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (10, 1, 0, 9, N'update-001.000.009.ddl', CAST(N'2015-08-24T07:18:27.000' AS DateTime))
INSERT [dbo].[SchemaChanges] ([id], [major], [minor], [release], [script_name], [applied]) VALUES (11, 1, 1, 1, N'update-001.001.001.ddl', CAST(N'2015-08-24T07:18:27.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[SchemaChanges] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (1, N'BOB', N'vtibob', N'GROUP10', 1)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (2, N'DJM', N'drdan', N'GROUP10', 1)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (3, N'GEORGE', N'gms', N'GROUP10', 1)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (4, N'MDB', N'maddog', N'GROUP10', 1)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (5, N'TODD', N'whowhat', N'GROUP10', 1)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (6, N'ROB', N'jethro', N'GROUP10', 1)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (7, N'OPERATOR', N'operator', N'GROUP01', 0)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (8, N'MAINT', N'maint', N'GROUP09', 0)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (9, N'Administrator', N'admin', N'GROUP09', 0)
INSERT [dbo].[Users] ([ID], [OpID], [Password], [GroupID], [IsLocked]) VALUES (10, N'VTI', N'vti', N'GROUP09', 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblGroupCommands_GroupID_CommandID]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblGroupCommands_GroupID_CommandID] ON [dbo].[GroupCommands]
(
	[GroupID] ASC,
	[CommandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblModelParameters_LastModified]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblModelParameters_LastModified] ON [dbo].[ModelParameters]
(
	[LastModified] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblModelParameters_ModelNo]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblModelParameters_ModelNo] ON [dbo].[ModelParameters]
(
	[ModelNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblModelParameters_ModelNo_ParameterID]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblModelParameters_ModelNo_ParameterID] ON [dbo].[ModelParameters]
(
	[ModelNo] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblModels_LastModified]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblModels_LastModified] ON [dbo].[Models]
(
	[LastModified] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblModels_LastModifiedBy]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblModels_LastModifiedBy] ON [dbo].[Models]
(
	[LastModifiedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ModelXRef_ModelNo]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_ModelXRef_ModelNo] ON [dbo].[ModelXRef]
(
	[ModelNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ModelXRef_ScannedChars]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_ModelXRef_ScannedChars] ON [dbo].[ModelXRef]
(
	[ScannedChars] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUsers_GroupID]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUsers_GroupID] ON [dbo].[Users]
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUsers_OpID]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tblUsers_OpID] ON [dbo].[Users]
(
	[OpID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUsers_Password]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUsers_Password] ON [dbo].[Users]
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutDefects_Category]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_Category] ON [dbo].[UutDefects]
(
	[Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutDefects_Defect]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_Defect] ON [dbo].[UutDefects]
(
	[Defect] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblUutDefects_UutRecordID]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_UutRecordID] ON [dbo].[UutDefects]
(
	[UutRecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutDefects_UutRecordID_Defect]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutDefects_UutRecordID_Defect] ON [dbo].[UutDefects]
(
	[UutRecordID] ASC,
	[Defect] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblUutRecordDetails_UutRecordID_DateTime]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecordDetails_UutRecordID_DateTime] ON [dbo].[UutRecordDetails]
(
	[UutRecordID] ASC,
	[DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblUutRecordDetails_UutRecordID_ID]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecordDetails_UutRecordID_ID] ON [dbo].[UutRecordDetails]
(
	[UutRecordID] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutRecordDetails_UutRecordID_TestName]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecordDetails_UutRecordID_TestName] ON [dbo].[UutRecordDetails]
(
	[UutRecordID] ASC,
	[Test] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblUutRecords_DateTime]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_DateTime] ON [dbo].[UutRecords]
(
	[DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutRecords_ModelNo]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_ModelNo] ON [dbo].[UutRecords]
(
	[ModelNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutRecords_SerialNo]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_SerialNo] ON [dbo].[UutRecords]
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblUutRecords_SerialNo_TestResult]    Script Date: 1/26/2021 9:20:16 AM ******/
CREATE NONCLUSTERED INDEX [IX_tblUutRecords_SerialNo_TestResult] ON [dbo].[UutRecords]
(
	[SerialNo] ASC,
	[TestResult] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_tblGroups_IsLocked]  DEFAULT ((1)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_tblUsers_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[UutRecordDetails] ADD  CONSTRAINT [DF_UutRecordDetails_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
ALTER TABLE [dbo].[UutRecords] ADD  CONSTRAINT [DF_UutRecords_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
ALTER TABLE [dbo].[GroupCommands]  WITH CHECK ADD  CONSTRAINT [FK_tblGroupCommands_tblGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[GroupCommands] CHECK CONSTRAINT [FK_tblGroupCommands_tblGroups]
GO
ALTER TABLE [dbo].[ModelParameters]  WITH CHECK ADD  CONSTRAINT [FK_ModelParameters_Models] FOREIGN KEY([ModelNo])
REFERENCES [dbo].[Models] ([ModelNo])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ModelParameters] CHECK CONSTRAINT [FK_ModelParameters_Models]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_tblUsers_tblGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_tblUsers_tblGroups]
GO
ALTER TABLE [dbo].[UutDefects]  WITH CHECK ADD  CONSTRAINT [FK_tblUutDefects_tblUutRecords] FOREIGN KEY([UutRecordID])
REFERENCES [dbo].[UutRecords] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UutDefects] CHECK CONSTRAINT [FK_tblUutDefects_tblUutRecords]
GO
ALTER TABLE [dbo].[UutRecordDetails]  WITH CHECK ADD  CONSTRAINT [FK_tblUutRecordDetails_tblUutRecords] FOREIGN KEY([UutRecordID])
REFERENCES [dbo].[UutRecords] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UutRecordDetails] CHECK CONSTRAINT [FK_tblUutRecordDetails_tblUutRecords]
GO
USE [master]
GO
ALTER DATABASE [VtiData] SET  READ_WRITE 
GO

-- Use VtiData DB for next command
use VtiData
GO

-- Grant only SELECT, UPDATE, INSERT, ad DELETE permissions to vtiuser
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name=N'vtiuser')
BEGIN
	CREATE USER vtiuser FOR LOGIN vtiuser
	GRANT SELECT ON SCHEMA::[dbo] TO vtiuser
	GRANT UPDATE ON SCHEMA::[dbo] TO vtiuser
	GRANT INSERT ON SCHEMA::[dbo] TO vtiuser
	GRANT DELETE ON SCHEMA::[dbo] TO vtiuser
END
GO
