CREATE TABLE [data].[Plugins] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Type]        INT            NOT NULL,
    [DisplayName] NVARCHAR (255) NOT NULL,
    [AssemblyName] NVARCHAR(255) NOT NULL,
    [TypeFullName] NVARCHAR(255) NOT NULL,
    CONSTRAINT [PK_Plugins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Plugins_AssemblyName_TypeFullName_Type] UNIQUE ([AssemblyName], [TypeFullName], [Type])
);