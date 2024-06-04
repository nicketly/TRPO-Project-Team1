CREATE TABLE [dbo].[History] (
    [ID_Ист]     INT             IDENTITY (1, 1) NOT NULL,
    [Код]        NCHAR (30)      NULL,
    [Дата]       SMALLDATETIME   NULL,
    [Цена]       DECIMAL (10, 2) NULL,
    [Количество] INT             NULL,
    [Сумма]      AS              ([Цена]*[Количество]),
    [Операция]   NCHAR (25)      NULL,
    PRIMARY KEY CLUSTERED ([ID_Ист] ASC)
);

