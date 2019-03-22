
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/10/2019 17:35:44
-- Generated from EDMX file: E:\毕业设计\ACE_Mall\ACE_Mall.Model\MallModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ACE_Mall];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Adm_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Adm_User];
GO
IF OBJECT_ID(N'[dbo].[Mall_Answer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mall_Answer];
GO
IF OBJECT_ID(N'[dbo].[Mall_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mall_Category];
GO
IF OBJECT_ID(N'[dbo].[Mall_Good]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mall_Good];
GO
IF OBJECT_ID(N'[dbo].[Mall_Good_Evaluation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mall_Good_Evaluation];
GO
IF OBJECT_ID(N'[dbo].[Mall_Good_Specification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mall_Good_Specification];
GO
IF OBJECT_ID(N'[dbo].[Mall_Question]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mall_Question];
GO
IF OBJECT_ID(N'[dbo].[My_Data]', 'U') IS NOT NULL
    DROP TABLE [dbo].[My_Data];
GO
IF OBJECT_ID(N'[dbo].[My_Order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[My_Order];
GO
IF OBJECT_ID(N'[dbo].[My_Order_Good]', 'U') IS NOT NULL
    DROP TABLE [dbo].[My_Order_Good];
GO
IF OBJECT_ID(N'[dbo].[My_Shopcart]', 'U') IS NOT NULL
    DROP TABLE [dbo].[My_Shopcart];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Adm_User'
CREATE TABLE [dbo].[Adm_User] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ReallyName] varchar(50)  NULL,
    [Account] varchar(50)  NULL,
    [Phone] varchar(20)  NULL,
    [Email] varchar(20)  NULL,
    [Password] varchar(20)  NULL,
    [Image] varchar(200)  NULL,
    [Birthday] datetime  NULL,
    [Sex] varchar(2)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'Mall_Answer'
CREATE TABLE [dbo].[Mall_Answer] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NULL,
    [QuestionID] int  NULL,
    [Answer] varchar(500)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'Mall_Category'
CREATE TABLE [dbo].[Mall_Category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'Mall_Good'
CREATE TABLE [dbo].[Mall_Good] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryID] int  NULL,
    [CoverImage] varchar(200)  NULL,
    [DetailImage] varchar(2000)  NULL,
    [Name] varchar(50)  NULL,
    [OriginalPrice] decimal(18,2)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'Mall_Good_Evaluation'
CREATE TABLE [dbo].[Mall_Good_Evaluation] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NULL,
    [GoodID] int  NULL,
    [Star] int  NULL,
    [Evaluation] varchar(500)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'Mall_Good_Specification'
CREATE TABLE [dbo].[Mall_Good_Specification] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Price] decimal(18,2)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'Mall_Question'
CREATE TABLE [dbo].[Mall_Question] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NULL,
    [Question] varchar(500)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'My_Data'
CREATE TABLE [dbo].[My_Data] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Email] varchar(50)  NULL,
    [Image] varchar(200)  NULL,
    [Account] varchar(50)  NULL,
    [Password] varchar(50)  NULL,
    [ReceiveName] varchar(50)  NULL,
    [ReceiveAddress] varchar(500)  NULL,
    [ReceivePhone] varchar(20)  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'My_Order'
CREATE TABLE [dbo].[My_Order] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [Orderno] varchar(50)  NOT NULL,
    [OrderState] int  NULL,
    [Name] varchar(50)  NULL,
    [Address] varchar(500)  NULL,
    [Phone] varchar(20)  NULL,
    [Note] varchar(200)  NULL,
    [CreateTime] datetime  NOT NULL,
    [DeliveryTime] datetime  NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'My_Order_Good'
CREATE TABLE [dbo].[My_Order_Good] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [OrderNo] varchar(50)  NULL,
    [Good_Specification_ID] int  NULL,
    [Good_Number] int  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- Creating table 'My_Shopcart'
CREATE TABLE [dbo].[My_Shopcart] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NULL,
    [SpecificationID] int  NULL,
    [Number] int  NULL,
    [CreateTime] datetime  NOT NULL,
    [IsDelete] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Adm_User'
ALTER TABLE [dbo].[Adm_User]
ADD CONSTRAINT [PK_Adm_User]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mall_Answer'
ALTER TABLE [dbo].[Mall_Answer]
ADD CONSTRAINT [PK_Mall_Answer]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mall_Category'
ALTER TABLE [dbo].[Mall_Category]
ADD CONSTRAINT [PK_Mall_Category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mall_Good'
ALTER TABLE [dbo].[Mall_Good]
ADD CONSTRAINT [PK_Mall_Good]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mall_Good_Evaluation'
ALTER TABLE [dbo].[Mall_Good_Evaluation]
ADD CONSTRAINT [PK_Mall_Good_Evaluation]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mall_Good_Specification'
ALTER TABLE [dbo].[Mall_Good_Specification]
ADD CONSTRAINT [PK_Mall_Good_Specification]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mall_Question'
ALTER TABLE [dbo].[Mall_Question]
ADD CONSTRAINT [PK_Mall_Question]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'My_Data'
ALTER TABLE [dbo].[My_Data]
ADD CONSTRAINT [PK_My_Data]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'My_Order'
ALTER TABLE [dbo].[My_Order]
ADD CONSTRAINT [PK_My_Order]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'My_Order_Good'
ALTER TABLE [dbo].[My_Order_Good]
ADD CONSTRAINT [PK_My_Order_Good]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'My_Shopcart'
ALTER TABLE [dbo].[My_Shopcart]
ADD CONSTRAINT [PK_My_Shopcart]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------