CREATE TABLE [dbo].[Friends] (
    [RequestedById] INT           NOT NULL,
    [RequestedToId] INT           NOT NULL,
    [CreatedAt]     DATETIME2 (7) NULL,
    [UpdatedAt]     DATETIME2 (7) NULL,
    [StatusId]      INT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED ([RequestedById] ASC, [RequestedToId] ASC),
    CONSTRAINT [FK_Friends_FriendStatusTypes_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[FriendStatusTypes] ([FriendStatusId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Friends_Users_RequestedById] FOREIGN KEY ([RequestedById]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Friends_Users_RequestedToId] FOREIGN KEY ([RequestedToId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Friends_RequestedToId]
    ON [dbo].[Friends]([RequestedToId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Friends_StatusId]
    ON [dbo].[Friends]([StatusId] ASC);

