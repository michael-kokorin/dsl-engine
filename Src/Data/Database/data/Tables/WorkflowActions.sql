CREATE TABLE [data].[WorkflowActions]
(
    [Id] BIGINT NOT NULL IDENTITY, 
    [Key] NVARCHAR(100) NOT NULL, 
    [DisplayName] NVARCHAR(400) NOT NULL, 
    CONSTRAINT [PK_WorkflowActions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_WorkflowActions_Key] UNIQUE ([Key])
)