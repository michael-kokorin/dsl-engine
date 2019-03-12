CREATE TABLE [system].[PackageVersions] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Version]   NVARCHAR (20)  NOT NULL,
    [Module]    NVARCHAR (400) NOT NULL,
    [Installed] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_PackageVersions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_PackageVersions_Module_Version] UNIQUE NONCLUSTERED ([Module] ASC, [Version] ASC)
);



