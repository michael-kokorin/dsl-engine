CREATE TABLE [queue].[Queue] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Type]        NVARCHAR (32) NOT NULL,
    [Created]     DATETIME2 (7) NOT NULL,
    [IsProcessed] BIT           DEFAULT ((0)) NOT NULL,
    [Processed]   DATETIME2 (7) NULL,
    [Body]        XML           NOT NULL,
    CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

