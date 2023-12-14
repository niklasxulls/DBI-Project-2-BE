CREATE TABLE [dbo].[ChatHistoryEntries] (
    [UserId]    INT           NOT NULL,
    [ChatId]    INT           NOT NULL,
    [CreatedAt] DATETIME2 (7) NULL,
    [UpdatedAt] DATETIME2 (7) NULL,
    CONSTRAINT [PK_ChatHistoryEntries] PRIMARY KEY CLUSTERED ([UserId] ASC, [ChatId] ASC),
    CONSTRAINT [FK_ChatHistoryEntries_Chats_ChatId] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([ChatId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChatHistoryEntries_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ChatHistoryEntries_ChatId]
    ON [dbo].[ChatHistoryEntries]([ChatId] ASC);

