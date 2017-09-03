
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/03/2017 17:04:44
-- Generated from EDMX file: D:\Mega\Documents\Svastara\Programiranje\ASP.NET\fakturiska\Fakturiska.Database\FakturiskaDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Fakturiska];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Invoice_Payer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_Invoice_Payer];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoice_Priority]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_Invoice_Priority];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoice_Reciever]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_Invoice_Reciever];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoices_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_Invoices_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_User_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_User_Role];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Invoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoices];
GO
IF OBJECT_ID(N'[dbo].[Priorities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Priorities];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [CompanyId] int IDENTITY(1,1) NOT NULL,
    [CompanyUId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [PhoneNumber] nvarchar(50)  NOT NULL,
    [FaxNumber] nvarchar(50)  NOT NULL,
    [Address] nvarchar(50)  NOT NULL,
    [Website] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [PersonalNumber] nvarchar(50)  NOT NULL,
    [PIB] nvarchar(50)  NOT NULL,
    [MIB] nvarchar(50)  NOT NULL,
    [AccountNumber] nvarchar(50)  NOT NULL,
    [BankCode] nvarchar(50)  NOT NULL,
    [DeleteDate] datetime  NULL
);
GO

-- Creating table 'Invoices'
CREATE TABLE [dbo].[Invoices] (
    [InvoiceId] int IDENTITY(1,1) NOT NULL,
    [InvoiceUId] uniqueidentifier  NOT NULL,
    [UserId] int  NOT NULL,
    [Date] datetime  NULL,
    [InvoiceEstimate] int  NULL,
    [InvoiceTotal] int  NULL,
    [Incoming] int  NULL,
    [Paid] int  NULL,
    [Risk] int  NULL,
    [Sum] int  NULL,
    [PriorityId] int  NULL,
    [ReceiverId] int  NULL,
    [PayerId] int  NULL,
    [FilePath] nvarchar(max)  NOT NULL,
    [Archive] int  NULL,
    [DeleteDate] datetime  NULL
);
GO

-- Creating table 'Priorities'
CREATE TABLE [dbo].[Priorities] (
    [PriorityId] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserUId] uniqueidentifier  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [RoleId] int  NOT NULL,
    [DeleteDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CompanyId] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([CompanyId] ASC);
GO

-- Creating primary key on [InvoiceId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [PK_Invoices]
    PRIMARY KEY CLUSTERED ([InvoiceId] ASC);
GO

-- Creating primary key on [PriorityId] in table 'Priorities'
ALTER TABLE [dbo].[Priorities]
ADD CONSTRAINT [PK_Priorities]
    PRIMARY KEY CLUSTERED ([PriorityId] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PayerId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_Invoice_Payer]
    FOREIGN KEY ([PayerId])
    REFERENCES [dbo].[Companies]
        ([CompanyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_Payer'
CREATE INDEX [IX_FK_Invoice_Payer]
ON [dbo].[Invoices]
    ([PayerId]);
GO

-- Creating foreign key on [ReceiverId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_Invoice_Reciever]
    FOREIGN KEY ([ReceiverId])
    REFERENCES [dbo].[Companies]
        ([CompanyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_Reciever'
CREATE INDEX [IX_FK_Invoice_Reciever]
ON [dbo].[Invoices]
    ([ReceiverId]);
GO

-- Creating foreign key on [PriorityId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_Invoice_Priority]
    FOREIGN KEY ([PriorityId])
    REFERENCES [dbo].[Priorities]
        ([PriorityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_Priority'
CREATE INDEX [IX_FK_Invoice_Priority]
ON [dbo].[Invoices]
    ([PriorityId]);
GO

-- Creating foreign key on [UserId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_Invoices_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoices_Users'
CREATE INDEX [IX_FK_Invoices_Users]
ON [dbo].[Invoices]
    ([UserId]);
GO

-- Creating foreign key on [RoleId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_Role'
CREATE INDEX [IX_FK_User_Role]
ON [dbo].[Users]
    ([RoleId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------