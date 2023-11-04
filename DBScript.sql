CREATE DATABASE [MediConnect]
GO
USE [MediConnect]
GO
/****** Object:  Table [dbo].[DonorData]    Script Date: 15-09-2023 08:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonorData](
	[DonorID] [int] IDENTITY(1,1) NOT NULL,
	[Organs] [nvarchar](350) NULL,
	[MedInfo] [nvarchar](350) NULL,
	[Name] [nvarchar](50) NULL,
	[Relationship] [nvarchar](50) NULL,
	[Contact] [nvarchar](50) NULL,
	[Address] [nvarchar](150) NULL,
	[Sign] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_DonorData] PRIMARY KEY CLUSTERED 
(
	[DonorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecipientData]    Script Date: 15-09-2023 08:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecipientData](
	[RecipientID] [int] IDENTITY(1,1) NOT NULL,
	[OrgansInfo] [nvarchar](150) NULL,
	[BloodType] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Relationship] [nvarchar](50) NULL,
	[Contact] [nvarchar](50) NULL,
	[Address] [nvarchar](150) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_RecipientData] PRIMARY KEY CLUSTERED 
(
	[RecipientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRegistration]    Script Date: 15-09-2023 08:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRegistration](
	[RegID] [int] IDENTITY(1,1) NOT NULL,
	[Firstname] [nvarchar](150) NULL,
	[Lastname] [nvarchar](150) NULL,
	[Gender] [char](2) NULL,
	[DOB] [date] NULL,
	[BloodGroup] [nvarchar](5) NULL,
	[Email] [nvarchar](150) NULL,
	[Address] [nvarchar](250) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Zipcode] [nvarchar](50) NULL,
	[Username] [nvarchar](150) NULL,
	[Password] [nvarchar](100) NULL,
	[ConfirmPassword] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Feedback]    Script Date: 15-09-2023 08:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Ratings] [int] NOT NULL,
	[Feedback] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL,
	CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
	(
	[Id] ASC
)
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Contact]    Script Date: 15-09-2023 08:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Email] [nvarchar](250) NULL,
	[Name] [nvarchar](250) NULL,
	[Message] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL,
	CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
	(
	[Id] ASC
)
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Hospital]    Script Date: 15-09-2023 08:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hospital](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) Not NULL,
	[Address] [nvarchar](250) Not NULL,
	[GoogleMapLink] [nvarchar](250) Not NULL,
	[DistanceInKm] [int] Not NULL,
	[CreatedDate] [datetime] NOT NULL,
	CONSTRAINT [PK_Hospital] PRIMARY KEY CLUSTERED 
	(
	[Id] ASC
)
) ON [PRIMARY]
GO
/**** Seeding to Table [dbo].[Hospital] *****/
Insert INTO [dbo].[Hospital](Name, Address, GoogleMapLink, DistanceInKm, CreatedDate) 
VALUES ('', '', '', 4, DATE.)