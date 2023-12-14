CREATE TABLE [dbo].[Chats] (
    [ChatId]    INT           IDENTITY (1, 1) NOT NULL,
    [UserOneId] INT           NOT NULL,
    [UserTwoId] INT           NOT NULL,
    [CreatedAt] DATETIME2 (7) NULL,
    [UpdatedAt] DATETIME2 (7) NULL,
    CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED ([ChatId] ASC),
    CONSTRAINT [FK_Chats_Users_UserOneId] FOREIGN KEY ([UserOneId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Chats_Users_UserTwoId] FOREIGN KEY ([UserTwoId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Chats_UserOneId]
    ON [dbo].[Chats]([UserOneId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Chats_UserTwoId]
    ON [dbo].[Chats]([UserTwoId] ASC);

