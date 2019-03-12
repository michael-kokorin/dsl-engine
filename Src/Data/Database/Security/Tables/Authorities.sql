CREATE TABLE [security].[Authorities] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Key]         NVARCHAR (255) NOT NULL,
    [DisplayName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Authorities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Authorities_Key] UNIQUE NONCLUSTERED ([Key] ASC)
);