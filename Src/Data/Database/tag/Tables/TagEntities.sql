CREATE TABLE [tag].[TagEntities] (
    [Id]       BIGINT IDENTITY (1, 1) NOT NULL,
    [TagId]    BIGINT NOT NULL,
    [TableId]  BIGINT NOT NULL,
    [EntityId] BIGINT NOT NULL,
    CONSTRAINT [PK_TagEntities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TagEntities_Tables_TableId] FOREIGN KEY ([TableId]) REFERENCES [system].[Tables] ([Id]),
    CONSTRAINT [FK_TagEntities_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [tag].[Tags] ([Id]),
    CONSTRAINT [UK_TagEntities_TagId_TableId_EntityId] UNIQUE NONCLUSTERED ([TagId] ASC, [TableId] ASC, [EntityId] ASC)
);

