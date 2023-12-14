CREATE TABLE [dbo].[MessageStatusTypes] (
    [MessageStatusId] INT            NOT NULL,
    [Name]            NVARCHAR (50)  NOT NULL,
    [Desc]            NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_MessageStatusTypes] PRIMARY KEY CLUSTERED ([MessageStatusId] ASC)
);

