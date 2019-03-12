CREATE TABLE [security].[Users] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Sid]         NVARCHAR (185) NOT NULL,
    [Login]       NVARCHAR (104) NOT NULL,
    [DisplayName] NVARCHAR (MAX) NOT NULL,
    [IsActive]    BIT            DEFAULT ((1)) NOT NULL,
    [Email] NVARCHAR(100) NULL, 

    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Users_Sid] UNIQUE NONCLUSTERED ([Sid] ASC)
);