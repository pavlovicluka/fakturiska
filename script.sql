/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [master]
GO
/****** Object:  Database [FakturiskaDB]    Script Date: 04-Sep-17 09:19:40 ******/
CREATE DATABASE [FakturiskaDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FakturiskaDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\FakturiskaDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FakturiskaDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\FakturiskaDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FakturiskaDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FakturiskaDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FakturiskaDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FakturiskaDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FakturiskaDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FakturiskaDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FakturiskaDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FakturiskaDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FakturiskaDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FakturiskaDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FakturiskaDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FakturiskaDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FakturiskaDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FakturiskaDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FakturiskaDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FakturiskaDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FakturiskaDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FakturiskaDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FakturiskaDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FakturiskaDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FakturiskaDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FakturiskaDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FakturiskaDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FakturiskaDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FakturiskaDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FakturiskaDB] SET  MULTI_USER 
GO
ALTER DATABASE [FakturiskaDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FakturiskaDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FakturiskaDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FakturiskaDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FakturiskaDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FakturiskaDB] SET QUERY_STORE = OFF
GO
USE [FakturiskaDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [FakturiskaDB]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 04-Sep-17 09:19:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyUId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[FaxNumber] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Website] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PersonalNumber] [nvarchar](50) NOT NULL,
	[PIB] [nvarchar](50) NOT NULL,
	[MIB] [nvarchar](50) NOT NULL,
	[AccountNumber] [nvarchar](50) NOT NULL,
	[BankCode] [nvarchar](50) NOT NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_LegalPerson] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 04-Sep-17 09:19:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceUId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
	[Date] [date] NULL,
	[InvoiceEstimate] [int] NULL,
	[InvoiceTotal] [int] NULL,
	[Incoming] [int] NULL,
	[Paid] [int] NULL,
	[Risk] [int] NULL,
	[Sum] [int] NULL,
	[PriorityId] [int] NULL,
	[ReceiverId] [int] NULL,
	[PayerId] [int] NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[Archive] [int] NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Priorities]    Script Date: 04-Sep-17 09:19:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Priorities](
	[PriorityId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Priority] PRIMARY KEY CLUSTERED 
(
	[PriorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 04-Sep-17 09:19:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RoleING] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 04-Sep-17 09:19:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserUId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[DeleteDate] [datetime] NULL,
 CONSTRAINT [PK_UserING] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Payer] FOREIGN KEY([PayerId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoice_Payer]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Priority] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priorities] ([PriorityId])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoice_Priority]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Reciever] FOREIGN KEY([ReceiverId])
REFERENCES [dbo].[Companies] ([CompanyId])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoice_Reciever]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [FakturiskaDB] SET  READ_WRITE 
GO
