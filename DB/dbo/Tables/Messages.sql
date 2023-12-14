CREATE TABLE [dbo].[Messages] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [Content]             NVARCHAR (1000) NULL,
    [UserOneIsSender]     BIT             NOT NULL,
    [ChatId]              INT             NOT NULL,
    [MessageStatusId]     INT             NOT NULL,
    [CreatedAt]           DATETIME2 (7)   NULL,
    [UpdatedAt]           DATETIME2 (7)   NULL,
    [MessageUserStatusId] INT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Chats_ChatId] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([ChatId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_MessageStatusTypes_MessageStatusId] FOREIGN KEY ([MessageStatusId]) REFERENCES [dbo].[MessageStatusTypes] ([MessageStatusId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_MessageUserStatusTypes_MessageUserStatusId] FOREIGN KEY ([MessageUserStatusId]) REFERENCES [dbo].[MessageUserStatusTypes] ([MessageUserStatusId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_ChatId]
    ON [dbo].[Messages]([ChatId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_MessageStatusId]
    ON [dbo].[Messages]([MessageStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_MessageUserStatusId]
    ON [dbo].[Messages]([MessageUserStatusId] ASC);

