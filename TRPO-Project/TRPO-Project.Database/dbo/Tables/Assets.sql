CREATE TABLE [dbo].[Assets] (
    [Code] NCHAR (32) NOT NULL,
    [Name] NCHAR (64) NULL,
    [Type] NCHAR (32) NULL,
    PRIMARY KEY CLUSTERED ([Code] ASC)
);

