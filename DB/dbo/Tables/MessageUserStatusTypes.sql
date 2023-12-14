CREATE TABLE [dbo].[MessageUserStatusTypes] (
    [MessageUserStatusId] INT            NOT NULL,
    [Name]                NVARCHAR (50)  NOT NULL,
    [Desc]                NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_MessageUserStatusTypes] PRIMARY KEY CLUSTERED ([MessageUserStatusId] ASC)
);

