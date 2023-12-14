CREATE TABLE [dbo].[FriendStatusTypes] (
    [FriendStatusId] INT            NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [Desc]           NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_FriendStatusTypes] PRIMARY KEY CLUSTERED ([FriendStatusId] ASC)
);

