CREATE TABLE [dbo].[History]
(
	[ID_Ист] INT NOT NULL PRIMARY KEY, 
    [Наименование] NCHAR(70) NULL, 
    [Тип] NCHAR(20) NULL, 
    [Дата] SMALLDATETIME NULL, 
    [Цена] DECIMAL(10, 2) NULL, 
    [Количество] INT NULL, 
    [Сумма] AS ([Цена]*[Количество]), 
    [Операция] NCHAR(25) NULL
)
