CREATE TABLE [data].[QueryEntityNames]
(
    [Id] BIGINT NOT NULL IDENTITY, 
    [Key] NVARCHAR(100) NOT NULL, 
    [TypeName] NVARCHAR(400) NOT NULL, 
    [AssemblyName] NVARCHAR(400) NOT NULL, 
    CONSTRAINT [PK_QueryEntityNames] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_QueryEntityNames_Key] UNIQUE ([Key])
)