CREATE TABLE [dbo].[UserStatusTypes] (
    [UserStatusId] INT            NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Desc]         NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_UserStatusTypes] PRIMARY KEY CLUSTERED ([UserStatusId] ASC)
);

